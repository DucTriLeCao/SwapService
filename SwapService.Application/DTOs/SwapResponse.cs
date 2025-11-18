namespace SwapService.Application.DTOs;

public class SwapResponse
{
    public Guid SwapId { get; set; }
    public Guid? BookingId { get; set; }
    public Guid DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid StationId { get; set; }
    public Guid? StaffId { get; set; }
    public Guid? BatteryOutId { get; set; }
    public Guid BatteryInId { get; set; }
    public DateTime SwapStartTime { get; set; }
    public DateTime? SwapEndTime { get; set; }
    public int? DurationMinutes { get; set; }
    public int? BatteryOutChargeLevel { get; set; }
    public int? BatteryInChargeLevel { get; set; }
    public int? OdometerReading { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
