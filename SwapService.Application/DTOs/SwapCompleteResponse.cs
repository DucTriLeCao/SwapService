namespace SwapService.Application.DTOs;

public class SwapCompleteResponse
{
    public Guid SwapId { get; set; }
    public string Status { get; set; }
    public DateTime SwapEndTime { get; set; }
    public int DurationMinutes { get; set; }
    public BatteryInfo BatteryOut { get; set; }
    public BatteryInfo BatteryIn { get; set; }
}

public class BatteryInfo
{
    public Guid BatteryId { get; set; }
    public int ChargeLevel { get; set; }
    public string Status { get; set; }
}
