using SwapService.Application.DTOs;

namespace SwapService.Application.Interfaces;

public interface ISwapService
{
    Task<SwapStartResponse> StartSwapAsync(SwapStartRequest request, StaffContext staffContext);
    Task<SwapCompleteResponse> CompleteSwapAsync(SwapCompleteRequest request, StaffContext staffContext);
    Task<PaymentResponse> ProcessPaymentAsync(SwapPaymentRequest request, StaffContext staffContext);
    Task<SwapStatusResponse> GetSwapStatusAsync(Guid swapId, StaffContext staffContext);
    Task<SwapHistoryResponse> GetSwapHistoryAsync(SwapHistoryRequest request, StaffContext staffContext);
    Task<SwapListResponse> GetOngoingSwapsAsync(StaffContext staffContext);
    Task<SwapCancelResponse> CancelSwapAsync(Guid swapId, StaffContext staffContext);
    Task<SwapListAllResponse> GetAllSwapsAsync(StaffContext staffContext);
}
