namespace SwapService.Application.DTOs;

public class PaymentResponse
{
    public Guid PaymentId { get; set; }
    public Guid DriverId { get; set; }
    public Guid? SwapId { get; set; }
    public string PaymentType { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public string TransactionRef { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Description { get; set; }
}
