namespace SwapService.Application.DTOs;

public class SwapListResponse
{
    public List<SwapListItem> OngoingSwaps { get; set; }
    public int TotalCount { get; set; }
}

public class SwapListItem
{
    public Guid SwapId { get; set; }
    public Guid DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime SwapStartTime { get; set; }
    public string Status { get; set; }
    public Guid BatteryInId { get; set; }
    public int BatteryInChargeLevel { get; set; }
    public Guid? StaffId { get; set; }
}
