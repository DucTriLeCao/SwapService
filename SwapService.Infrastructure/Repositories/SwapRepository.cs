using Microsoft.EntityFrameworkCore;
using SwapService.Domain.Models;
using SwapService.Infrastructure.Interfaces;

namespace SwapService.Infrastructure.Repositories;

public class SwapRepository : ISwapRepository
{
    private readonly ev_battery_swapContext _context;

    public SwapRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<Swap> GetByIdAsync(Guid swapId)
    {
        return await _context.Swaps
            .Include(s => s.Payments)
            .Include(s => s.Booking)
            .FirstOrDefaultAsync(s => s.SwapId == swapId);
    }

    public async Task<IEnumerable<Swap>> GetByDriverIdAsync(Guid driverId)
    {
        return await _context.Swaps
            .Where(s => s.DriverId == driverId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Swap>> GetByStationIdAsync(Guid stationId)
    {
        return await _context.Swaps
            .Where(s => s.StationId == stationId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Swap> CreateAsync(Swap swap)
    {
        await _context.Swaps.AddAsync(swap);
        await _context.SaveChangesAsync();
        return swap;
    }

    public async Task<IEnumerable<Swap>> GetAllAsync()
    {
        return await _context.Swaps
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<Swap> UpdateAsync(Swap swap)
    {
        _context.Swaps.Update(swap);
        await _context.SaveChangesAsync();
        return swap;
    }
}
