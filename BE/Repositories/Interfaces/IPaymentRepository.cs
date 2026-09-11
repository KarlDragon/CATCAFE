using BE.Models;
using BE.DTOs;
namespace BE.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<int> CreatePaymentAsync(Payment payment);
    Task<bool> UpdatePaymentAsync(UpdatePaymentDTO updatePaymentDTO);
    Task<bool> IsPaymentExist(int bookingId);
    Task<IEnumerable<Payment>> GetAllPaymentsAsync(int userId);
}