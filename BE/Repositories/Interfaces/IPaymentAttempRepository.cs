namespace BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
public interface IPaymentAttempRepository
{
    public Task<int> CreatePaymentAttemptAsync(PaymentAttempt paymentAttempt);
    public Task<bool> UpdatePaymentAttemptAsync(UpdatePaymentAttemptDTO updatePaymentAttemptDTO);   
    
}