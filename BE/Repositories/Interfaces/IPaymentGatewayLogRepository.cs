namespace BE.Repositories.Interfaces;
using BE.Models;
public interface IPaymentGatewayLogRepository
{
    public Task<bool> CreatePaymentGatewayLogAsync(PaymentGatewayLog log);
}