namespace SwapService.Application.DTOs;

public class SwapListAllResponse
{
    public List<SwapDetailItem> Swaps { get; set; }
    public int TotalCount { get; set; }
}

public class SwapDetailItem
{
    public Guid SwapId { get; set; }
    public Guid DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid StationId { get; set; }
    public Guid? StaffId { get; set; }
    public Guid BatteryInId { get; set; }
    public Guid? BatteryOutId { get; set; }
    public int? BatteryOutChargeLevel { get; set; }
    public string Status { get; set; }
    public DateTime SwapStartTime { get; set; }
    public DateTime? SwapEndTime { get; set; }
    public int? DurationMinutes { get; set; }
    public Guid? BookingId { get; set; }
}
