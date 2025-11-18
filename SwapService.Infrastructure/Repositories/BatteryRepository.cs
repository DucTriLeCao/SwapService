using Microsoft.EntityFrameworkCore;
using SwapService.Domain.Models;
using SwapService.Infrastructure.Interfaces;

namespace SwapService.Infrastructure.Repositories;

public class BatteryRepository : IBatteryRepository
{
    private readonly ev_battery_swapContext _context;

    public BatteryRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<Battery> GetByIdAsync(Guid batteryId)
    {
        return await _context.Batteries.FindAsync(batteryId);
    }

    public async Task<Battery> UpdateAsync(Battery battery)
    {
        battery.UpdatedAt = DateTime.UtcNow;
        _context.Batteries.Update(battery);
        await _context.SaveChangesAsync();
        return battery;
    }

    public async Task<IEnumerable<Battery>> GetAvailableBatteriesAsync(Guid stationId)
    {
        return await _context.Batteries
            .Where(b => b.StationId == stationId && b.Status == "Available")
            .OrderByDescending(b => b.ChargeLevel)
            .ToListAsync();
    }
}
