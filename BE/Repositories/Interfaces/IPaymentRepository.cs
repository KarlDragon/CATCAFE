using BE.Models;

namespace BE.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<bool> CreatePaymentAsync(Payment payment);
    Task<bool> UpdatePaymentAsync(int PaymentID, PaymentStatus? paymentStatus, DateTime? paidAt, int? successfulAttemptId);
    Task<IEnumerable<Payment>> GetAllPaymentsAsync(int userId);
}