namespace BE.Services.Interfaces;
using BE.DTOs;
public interface IPaymentService
{
    Task<string> CreatePaymentAsync(CreatePaymentDTO createPaymentDTO, CancellationToken cancellationToken = default);
}