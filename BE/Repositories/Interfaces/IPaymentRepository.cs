using BE.Models;
using BE.DTOs;
namespace BE.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<int> CreatePaymentAsync(Payment payment);
    Task<bool> UpdatePaymentAsync(UpdatePaymentDTO updatePaymentDTO);
    Task<int> GetPaymentIdWithBookingId(int bookingId);// return -1 if no payment found
    Task<IEnumerable<Payment>> GetAllPaymentsAsync(int userId);
}