using SwapService.Application.DTOs;

namespace SwapService.Application.Interfaces;

public interface ISwapService
{
    Task<SwapStartResponse> StartSwapAsync(SwapStartRequest request, Guid userId);
    Task<SwapCompleteResponse> CompleteSwapAsync(SwapCompleteRequest request, Guid userId);
    Task<PaymentResponse> ProcessPaymentAsync(SwapPaymentRequest request, Guid userId);
    Task<SwapStatusResponse> GetSwapStatusAsync(Guid swapId, Guid userId);
    Task<SwapHistoryResponse> GetSwapHistoryAsync(SwapHistoryRequest request, Guid userId);
    Task<SwapListResponse> GetOngoingSwapsAsync(Guid userId);
    Task<SwapCancelResponse> CancelSwapAsync(Guid swapId, Guid userId);
    Task<SwapListAllResponse> GetAllSwapsAsync(Guid userId);
}
