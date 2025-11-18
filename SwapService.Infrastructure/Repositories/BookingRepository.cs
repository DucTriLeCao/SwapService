using Microsoft.EntityFrameworkCore;
using SwapService.Domain.Models;
using SwapService.Infrastructure.Interfaces;

namespace SwapService.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ev_battery_swapContext _context;

    public BookingRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<Booking> GetByIdAsync(Guid bookingId)
    {
        return await _context.Bookings.FindAsync(bookingId);
    }

    public async Task<Booking> UpdateAsync(Booking booking)
    {
        booking.UpdatedAt = DateTime.UtcNow;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
        return booking;
    }
}
