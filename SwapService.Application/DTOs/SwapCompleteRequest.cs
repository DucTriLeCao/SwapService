namespace SwapService.Application.DTOs;

public class SwapCompleteRequest
{
    public Guid SwapId { get; set; }
    public Guid BatteryOutId { get; set; }
    public int BatteryOutChargeLevel { get; set; }
    public string BatteryOutStatus { get; set; }
    public decimal? BatteryOutSoh { get; set; }
    public int? OdometerReading { get; set; }
    public string Notes { get; set; }
}
