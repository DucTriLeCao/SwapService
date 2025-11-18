namespace SwapService.Application.DTOs;

public class BatteryConditionResponse
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
