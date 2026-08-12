namespace BE.Services.Interfaces;
using BE.DTOs;
public interface IPaymentService
{
    public Task CreatePaymentAsync(CreatePaymentDTO createPaymentDTO);
}