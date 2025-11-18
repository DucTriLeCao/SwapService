using SwapService.Domain.Models;

namespace SwapService.Infrastructure.Interfaces;

public interface ISwapRepository
{
    Task<Swap> GetByIdAsync(Guid swapId);
    Task<IEnumerable<Swap>> GetByDriverIdAsync(Guid driverId);
    Task<IEnumerable<Swap>> GetByStationIdAsync(Guid stationId);
    Task<IEnumerable<Swap>> GetAllAsync();
    Task<Swap> CreateAsync(Swap swap);
    Task<Swap> UpdateAsync(Swap swap);
}
