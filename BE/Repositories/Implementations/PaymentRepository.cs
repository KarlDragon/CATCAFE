namespace BE.Repositories.Implementations;
using BE.Models;
using BE.Repositories.Interfaces;
using BE.DTOs;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;
    public PaymentRepository( AppDbContext context)
    {
        _context = context;
    }
    public async Task<int> CreatePaymentAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment.PaymentID;
    }

    public async Task<bool> UpdatePaymentAsync(UpdatePaymentDTO updatePaymentDTO)
    {
        var payment = await _context.Payments.FindAsync(updatePaymentDTO.PaymentID);
        if (payment == null) return false;

        payment.Status = updatePaymentDTO.PaymentStatus ?? payment.Status;
        payment.PaidAt = updatePaymentDTO.PaidAt ?? payment.PaidAt;
        payment.SuccessfulAttemptId = updatePaymentDTO.SuccessfulAttemptId ?? payment.SuccessfulAttemptId;

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