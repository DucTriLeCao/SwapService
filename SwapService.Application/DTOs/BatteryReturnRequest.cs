namespace SwapService.Application.DTOs;

public class BatteryReturnRequest
{
    public Guid SwapId { get; set; }
    public Guid BatteryId { get; set; }
    public int ChargeLevel { get; set; }
    public decimal SohPercentage { get; set; }
    public bool IsHealthy { get; set; }
    public string ConditionNotes { get; set; }
}
