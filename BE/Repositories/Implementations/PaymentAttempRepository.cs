namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
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

    public async Task<bool> UpdatePaymentAttemptAsync(int attemptId, PaymentAttemptStatus? status, string? transactionId, int? resultCode, string? payUrl)
    {
        var paymentAttempt = await _context.PaymentAttempts.FindAsync(attemptId);
        if (paymentAttempt == null) return false;

        paymentAttempt.Status = status ?? paymentAttempt.Status;
        paymentAttempt.TransactionId = transactionId ?? paymentAttempt.TransactionId;
        paymentAttempt.ResultCode = resultCode ?? paymentAttempt.ResultCode;
        paymentAttempt.PayUrl = payUrl ?? paymentAttempt.PayUrl;

        return await _context.SaveChangesAsync() > 0;
    }
}