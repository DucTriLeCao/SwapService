using SwapService.Application.DTOs;
using SwapService.Application.Interfaces;
using SwapService.Domain.Constants;
using SwapService.Domain.Models;
using SwapService.Infrastructure.Interfaces;

namespace SwapService.Infrastructure.Services;

public class SwapService : ISwapService
{
    private readonly ISwapRepository _swapRepository;
    private readonly IBatteryRepository _batteryRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBookingRepository _bookingRepository;

    public SwapService(
        ISwapRepository swapRepository,
        IBatteryRepository batteryRepository,
        IPaymentRepository paymentRepository,
        IBookingRepository bookingRepository)
    {
        _swapRepository = swapRepository;
        _batteryRepository = batteryRepository;
        _paymentRepository = paymentRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<SwapStartResponse> StartSwapAsync(SwapStartRequest request, StaffContext staffContext)
    {
        if (request.BookingId.HasValue && request.BookingId.Value != Guid.Empty)
        {
            var booking = await _bookingRepository.GetByIdAsync(request.BookingId.Value);
            if (booking != null && booking.Status == "pending")
            {
                booking.Status = "confirmed";
                await _bookingRepository.UpdateAsync(booking);
            }
        }

        var availableBatteries = await _batteryRepository.GetAvailableBatteriesAsync(staffContext.StationId);
        var batteryIn = availableBatteries.FirstOrDefault();
        
        if (batteryIn == null)
            throw new Exception("No available battery at this station");

        var swap = new Swap
        {
            SwapId = Guid.NewGuid(),
            BookingId = request.BookingId,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            StationId = staffContext.StationId,
            StaffId = staffContext.StaffId,
            BatteryInId = batteryIn.BatteryId,
            SwapStartTime = DateTime.UtcNow,
            Status = SwapStatus.InProgress,
            CreatedAt = DateTime.UtcNow
        };

        var createdSwap = await _swapRepository.CreateAsync(swap);

        batteryIn.Status = BatteryStatus.InUse;
        await _batteryRepository.UpdateAsync(batteryIn);

        return new SwapStartResponse
        {
            SwapId = createdSwap.SwapId,
            BatteryInId = batteryIn.BatteryId,
            Status = createdSwap.Status,
            SwapStartTime = createdSwap.SwapStartTime,
            BatteryInChargeLevel = batteryIn.ChargeLevel
        };
    }

    public async Task<SwapCompleteResponse> CompleteSwapAsync(SwapCompleteRequest request, StaffContext staffContext)
    {
        var swap = await _swapRepository.GetByIdAsync(request.SwapId);
        if (swap == null)
            throw new Exception("Swap not found");

        if (swap.StationId != staffContext.StationId)
            throw new UnauthorizedAccessException("You can only complete swaps in your station");

        swap.BatteryOutId = request.BatteryOutId;
        swap.BatteryOutChargeLevel = request.BatteryOutChargeLevel;
        swap.SwapEndTime = DateTime.UtcNow;
        swap.DurationMinutes = (int)(swap.SwapEndTime.Value - swap.SwapStartTime).TotalMinutes;
        swap.Status = SwapStatus.Completed;
        swap.Notes = request.Notes;

        await _swapRepository.UpdateAsync(swap);

        var batteryOut = await _batteryRepository.GetByIdAsync(request.BatteryOutId);
        if (batteryOut != null)
        {
            batteryOut.ChargeLevel = request.BatteryOutChargeLevel;
            if (request.BatteryOutSoh.HasValue)
            {
                batteryOut.SohPercentage = request.BatteryOutSoh.Value;
            }
            
            if (request.BatteryOutChargeLevel < 20)
            {
                batteryOut.Status = BatteryStatus.Charging;
            }
            else if (request.BatteryOutChargeLevel >= 80)
            {
                batteryOut.Status = BatteryStatus.Available;
            }
            else
            {
                batteryOut.Status = BatteryStatus.Charging;
            }
            
            batteryOut.LastSwapDate = DateTime.UtcNow;
            batteryOut.TotalCycles += 1;
            await _batteryRepository.UpdateAsync(batteryOut);
        }

        if (swap.BookingId.HasValue)
        {
            var booking = await _bookingRepository.GetByIdAsync(swap.BookingId.Value);
            if (booking != null)
            {
                booking.Status = "completed";
                await _bookingRepository.UpdateAsync(booking);
            }
        }

        var batteryIn = await _batteryRepository.GetByIdAsync(swap.BatteryInId);

        return new SwapCompleteResponse
        {
            SwapId = swap.SwapId,
            Status = swap.Status,
            SwapEndTime = swap.SwapEndTime.Value,
            DurationMinutes = swap.DurationMinutes.Value,
            BatteryOut = new BatteryInfo
            {
                BatteryId = batteryOut.BatteryId,
                ChargeLevel = batteryOut.ChargeLevel,
                Status = batteryOut.Status
            },
            BatteryIn = new BatteryInfo
            {
                BatteryId = batteryIn.BatteryId,
                ChargeLevel = batteryIn.ChargeLevel,
                Status = batteryIn.Status
            }
        };
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(SwapPaymentRequest request, StaffContext staffContext)
    {
        var swap = await _swapRepository.GetByIdAsync(request.SwapId);
        if (swap == null)
            throw new Exception("Swap not found");

        if (swap.StationId != staffContext.StationId)
            throw new UnauthorizedAccessException("You can only process payments for swaps in your station");

        var payment = new Payment
        {
            PaymentId = Guid.NewGuid(),
            DriverId = swap.DriverId,
            SwapId = swap.SwapId,
            PaymentType = "SwapFee",
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = PaymentStatus.Completed,
            TransactionRef = GenerateTransactionRef(),
            PaymentDate = DateTime.UtcNow,
            Description = request.Description ?? "Battery swap fee",
            CreatedAt = DateTime.UtcNow
        };

        var createdPayment = await _paymentRepository.CreateAsync(payment);

        return new PaymentResponse
        {
            PaymentId = createdPayment.PaymentId,
            DriverId = createdPayment.DriverId,
            SwapId = createdPayment.SwapId,
            PaymentType = createdPayment.PaymentType,
            Amount = createdPayment.Amount,
            PaymentMethod = createdPayment.PaymentMethod,
            PaymentStatus = createdPayment.PaymentStatus,
            TransactionRef = createdPayment.TransactionRef,
            PaymentDate = createdPayment.PaymentDate,
            Description = createdPayment.Description
        };
    }

    public async Task<SwapStatusResponse> GetSwapStatusAsync(Guid swapId, StaffContext staffContext)
    {
        var swap = await _swapRepository.GetByIdAsync(swapId);
        if (swap == null)
            throw new Exception("Swap not found");

        if (swap.StationId != staffContext.StationId)
            throw new UnauthorizedAccessException("You can only view swaps in your station");

        var batteryIn = await _batteryRepository.GetByIdAsync(swap.BatteryInId);

        return new SwapStatusResponse
        {
            SwapId = swap.SwapId,
            DriverId = swap.DriverId,
            VehicleId = swap.VehicleId,
            StationId = swap.StationId,
            StaffId = swap.StaffId,
            BatteryOutId = swap.BatteryOutId,
            BatteryInId = swap.BatteryInId,
            BatteryOutChargeLevel = swap.BatteryOutChargeLevel,
            BatteryInChargeLevel = batteryIn?.ChargeLevel,
            Status = swap.Status,
            SwapStartTime = swap.SwapStartTime,
            SwapEndTime = swap.SwapEndTime,
            DurationMinutes = swap.DurationMinutes
        };
    }

    public async Task<SwapHistoryResponse> GetSwapHistoryAsync(SwapHistoryRequest request, StaffContext staffContext)
    {
        var query = await _swapRepository.GetAllAsync();
        
        query = query.Where(s => s.StationId == staffContext.StationId).ToList();
        
        if (request.DriverId.HasValue)
            query = query.Where(s => s.DriverId == request.DriverId.Value).ToList();
        
        if (request.StartDate.HasValue)
            query = query.Where(s => s.SwapStartTime >= request.StartDate.Value).ToList();
        
        if (request.EndDate.HasValue)
            query = query.Where(s => s.SwapStartTime <= request.EndDate.Value).ToList();

        var totalCount = query.Count();
        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var items = query
            .OrderByDescending(s => s.SwapStartTime)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(s => new SwapHistoryItem
            {
                SwapId = s.SwapId,
                DriverId = s.DriverId,
                StationId = s.StationId,
                SwapStartTime = s.SwapStartTime,
                SwapEndTime = s.SwapEndTime,
                Status = s.Status,
                DurationMinutes = s.DurationMinutes,
                BatteryInId = s.BatteryInId,
                BatteryOutId = s.BatteryOutId,
                TotalAmount = _paymentRepository.GetBySwapIdAsync(s.SwapId).Result?.Amount
            })
            .ToList();

        return new SwapHistoryResponse
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = totalPages
        };
    }

    public async Task<SwapListResponse> GetOngoingSwapsAsync(StaffContext staffContext)
    {
        var allSwaps = await _swapRepository.GetAllAsync();
        var ongoingSwaps = allSwaps
            .Where(s => s.StationId == staffContext.StationId && s.Status == SwapStatus.InProgress)
            .OrderBy(s => s.SwapStartTime)
            .ToList();

        var items = new List<SwapListItem>();
        foreach (var swap in ongoingSwaps)
        {
            var batteryIn = await _batteryRepository.GetByIdAsync(swap.BatteryInId);
            items.Add(new SwapListItem
            {
                SwapId = swap.SwapId,
                DriverId = swap.DriverId,
                VehicleId = swap.VehicleId,
                SwapStartTime = swap.SwapStartTime,
                Status = swap.Status,
                BatteryInId = swap.BatteryInId,
                BatteryInChargeLevel = batteryIn?.ChargeLevel ?? 0,
                StaffId = swap.StaffId
            });
        }

        return new SwapListResponse
        {
            OngoingSwaps = items,
            TotalCount = items.Count
        };
    }

    public async Task<SwapCancelResponse> CancelSwapAsync(Guid swapId, StaffContext staffContext)
    {
        var swap = await _swapRepository.GetByIdAsync(swapId);
        if (swap == null)
            throw new Exception("Swap not found");

        if (swap.StationId != staffContext.StationId)
            throw new UnauthorizedAccessException("You can only cancel swaps in your station");

        if (swap.Status != SwapStatus.InProgress)
            throw new Exception($"Cannot cancel swap with status: {swap.Status}");

        swap.Status = SwapStatus.Cancelled;
        swap.SwapEndTime = DateTime.UtcNow;
        await _swapRepository.UpdateAsync(swap);

        var batteryIn = await _batteryRepository.GetByIdAsync(swap.BatteryInId);
        if (batteryIn != null)
        {
            batteryIn.Status = BatteryStatus.Available;
            await _batteryRepository.UpdateAsync(batteryIn);
        }

        if (swap.BookingId.HasValue)
        {
            var booking = await _bookingRepository.GetByIdAsync(swap.BookingId.Value);
            if (booking != null)
            {
                booking.Status = "cancelled";
                await _bookingRepository.UpdateAsync(booking);
            }
        }

        return new SwapCancelResponse
        {
            SwapId = swap.SwapId,
            Status = swap.Status,
            CancelledAt = swap.SwapEndTime.Value,
            Message = "Swap cancelled successfully"
        };
    }

    public async Task<SwapListAllResponse> GetAllSwapsAsync(StaffContext staffContext)
    {
        var allSwaps = await _swapRepository.GetAllAsync();
        
        var swapItems = allSwaps
            .Where(s => s.StationId == staffContext.StationId)
            .Select(s => new SwapDetailItem
        {
            SwapId = s.SwapId,
            DriverId = s.DriverId,
            VehicleId = s.VehicleId,
            StationId = s.StationId,
            StaffId = s.StaffId,
            BatteryInId = s.BatteryInId,
            BatteryOutId = s.BatteryOutId,
            BatteryOutChargeLevel = s.BatteryOutChargeLevel,
            Status = s.Status,
            SwapStartTime = s.SwapStartTime,
            SwapEndTime = s.SwapEndTime,
            DurationMinutes = s.DurationMinutes,
            BookingId = s.BookingId
        }).ToList();

        return new SwapListAllResponse
        {
            Swaps = swapItems,
            TotalCount = swapItems.Count
        };
    }

    private string GenerateTransactionRef()
    {
        return $"TXN-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
    }
}
