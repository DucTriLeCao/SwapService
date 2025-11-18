namespace SwapService.Domain.Entities;

public class BatteryCondition
{
    public Guid BatteryId { get; set; }
    public string Status { get; set; }
    public int ChargeLevel { get; set; }
    public decimal SohPercentage { get; set; }
    public int TotalCycles { get; set; }
    public DateTime? LastSwapDate { get; set; }
    public bool IsHealthy { get; set; }
    public string ConditionNotes { get; set; }
}
