using Microsoft.EntityFrameworkCore;
using SwapService.Domain.Models;
using SwapService.Infrastructure.Interfaces;

namespace SwapService.Infrastructure.Repositories;

public class StaffRepository : IStaffRepository
{
    private readonly ev_battery_swapContext _context;

    public StaffRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<Staff> GetByUserIdAsync(Guid userId)
    {
        return await _context.Staff
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    public async Task<Staff> GetByIdAsync(Guid staffId)
    {
        return await _context.Staff.FindAsync(staffId);
    }
}
