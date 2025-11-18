using SwapService.Domain.Models;

namespace SwapService.Infrastructure.Interfaces;

public interface IBatteryRepository
{
    Task<Battery> GetByIdAsync(Guid batteryId);
    Task<Battery> UpdateAsync(Battery battery);
    Task<IEnumerable<Battery>> GetAvailableBatteriesAsync(Guid stationId);
}
