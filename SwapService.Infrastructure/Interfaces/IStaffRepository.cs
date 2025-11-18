using SwapService.Domain.Models;

namespace SwapService.Infrastructure.Interfaces;

public interface IStaffRepository
{
    Task<Staff> GetByUserIdAsync(Guid userId);
    Task<Staff> GetByIdAsync(Guid staffId);
}
