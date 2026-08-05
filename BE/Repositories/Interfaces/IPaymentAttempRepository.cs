namespace BE.Repositories.Interfaces;
using BE.Models;
public interface IPaymentAttempRepository
{
    public Task<int> CreatePaymentAttemptAsync(PaymentAttempt paymentAttempt);
    public Task<bool> UpdatePaymentAttemptAsync(int attemptId, PaymentAttemptStatus? status, string? transactionId, int? resultCode, string? payUrl);   
    
}