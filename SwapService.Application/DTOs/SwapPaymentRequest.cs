namespace SwapService.Application.DTOs;

public class SwapPaymentRequest
{
    public Guid SwapId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string? Description { get; set; }
}
