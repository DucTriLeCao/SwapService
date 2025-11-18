using SwapService.Domain.Models;

namespace SwapService.Infrastructure.Interfaces;

public interface IBookingRepository
{
    Task<Booking> GetByIdAsync(Guid bookingId);
    Task<Booking> UpdateAsync(Booking booking);
}
