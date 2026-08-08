namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
public class PaymentAttempRepository : IPaymentAttempRepository
{
    private readonly AppDbContext _context;
    public PaymentAttempRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreatePaymentAttemptAsync(PaymentAttempt paymentAttempt)
    {
        _context.PaymentAttempts.Add(paymentAttempt);
        await _context.SaveChangesAsync();
        return paymentAttempt.AttemptID;
    }

    public async Task<bool> UpdatePaymentAttemptAsync(UpdatePaymentAttemptDTO updatePaymentAttemptDTO)
    {
        var paymentAttempt = await _context.PaymentAttempts.FindAsync(updatePaymentAttemptDTO.AttemptId);
        if (paymentAttempt == null) return false;

        paymentAttempt.Status = updatePaymentAttemptDTO.Status ?? paymentAttempt.Status;
        paymentAttempt.TransactionId = updatePaymentAttemptDTO.TransactionId ?? paymentAttempt.TransactionId;
        paymentAttempt.ResultCode = updatePaymentAttemptDTO.ResultCode ?? paymentAttempt.ResultCode;
        paymentAttempt.PayUrl = updatePaymentAttemptDTO.PayUrl ?? paymentAttempt.PayUrl;

        return await _context.SaveChangesAsync() > 0;
    }
}