using BE.Models;

namespace BE.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<bool> CreatePayment(Payment payment);
    Task<bool> UpdatePayment(int PaymentID, PaymentStatus? paymentStatus, DateTime? paidAt, int? successfulAttemptId);
    Task<IEnumerable<Payment>> GetAllPayments(int userId);
}