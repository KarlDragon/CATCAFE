namespace BE.Repositories.Implementations;
using BE.Models;
using BE.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    public PaymentRepository( AppDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreatePaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        return await _context.SaveChangesAsync() > 0; 
    }

    public async Task<bool> UpdatePaymentAsync(int PaymentID, PaymentStatus? paymentStatus, DateTime? paidAt, int? successfulAttemptId)
    {
        var payment = await _context.Payments.FindAsync(PaymentID);
        if (payment == null) return false;

        payment.Status = paymentStatus ?? payment.Status;
        payment.PaidAt = paidAt ?? payment.PaidAt;
        payment.SuccessfulAttemptId = successfulAttemptId ?? payment.SuccessfulAttemptId;

        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync(int userId)
    {
        return await _context.Payments
                    .AsNoTracking()
                    .Include(p => p.Booking)
                    .Include(p => p.SuccessfulAttempt)
                    .Where(p => p.Booking!.UserID == userId)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
    }
}