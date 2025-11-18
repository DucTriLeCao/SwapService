namespace SwapService.Application.DTOs;

public class SwapCancelResponse
{
    public Guid SwapId { get; set; }
    public string Status { get; set; }
    public DateTime CancelledAt { get; set; }
    public string Message { get; set; }
}
