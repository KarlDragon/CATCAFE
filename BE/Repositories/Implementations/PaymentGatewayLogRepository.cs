namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;

public class PaymentGatewayLogRepository : IPaymentGatewayLogRepository
{
    private readonly AppDbContext _context;
    public PaymentGatewayLogRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CreatePaymentGatewayLogAsync(PaymentGatewayLog log)
    {
        _context.PaymentGatewayLogs.Add(log);
        return await _context.SaveChangesAsync() > 0;
    }
}