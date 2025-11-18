using SwapService.Domain.Models;

namespace SwapService.Infrastructure.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> CreateAsync(Payment payment);
    Task<Payment> GetByIdAsync(Guid paymentId);
    Task<Payment> GetBySwapIdAsync(Guid swapId);
    Task<IEnumerable<Payment>> GetAllBySwapIdAsync(Guid swapId);
}
