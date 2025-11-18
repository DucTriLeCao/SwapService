namespace SwapService.Application.DTOs;

public class SwapHistoryResponse
{
    public List<SwapHistoryItem> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}

public class SwapHistoryItem
{
    public Guid SwapId { get; set; }
    public Guid DriverId { get; set; }
    public Guid StationId { get; set; }
    public DateTime SwapStartTime { get; set; }
    public DateTime? SwapEndTime { get; set; }
    public string Status { get; set; }
    public int? DurationMinutes { get; set; }
    public Guid BatteryInId { get; set; }
    public Guid? BatteryOutId { get; set; }
    public decimal? TotalAmount { get; set; }
}
