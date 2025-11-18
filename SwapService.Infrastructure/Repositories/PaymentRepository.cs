using Microsoft.EntityFrameworkCore;
using SwapService.Domain.Models;
using SwapService.Infrastructure.Interfaces;

namespace SwapService.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly ev_battery_swapContext _context;

    public PaymentRepository(ev_battery_swapContext context)
    {
        _context = context;
    }

    public async Task<Payment> CreateAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> GetByIdAsync(Guid paymentId)
    {
        return await _context.Payments.FindAsync(paymentId);
    }

    public async Task<Payment> GetBySwapIdAsync(Guid swapId)
    {
        return await _context.Payments
            .Where(p => p.SwapId == swapId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Payment>> GetAllBySwapIdAsync(Guid swapId)
    {
        return await _context.Payments
            .Where(p => p.SwapId == swapId)
            .ToListAsync();
    }
}
