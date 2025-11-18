namespace SwapService.Application.DTOs;

public class SwapStartResponse
{
    public Guid SwapId { get; set; }
    public Guid BatteryInId { get; set; }
    public string Status { get; set; }
    public DateTime SwapStartTime { get; set; }
    public int BatteryInChargeLevel { get; set; }
}
