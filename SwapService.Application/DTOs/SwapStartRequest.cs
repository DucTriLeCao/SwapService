namespace SwapService.Application.DTOs;

public class SwapStartRequest
{
    public Guid DriverId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? BookingId { get; set; }
}
