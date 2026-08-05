namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Repositories.Interfaces;
using BE.DTOs;
using BE.Models;
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<bool> CreatePaymentAsync(CreatePaymentDTO createPaymentDTO)
    {
        var newPayment = new Payment
        {
            BookingID = createPaymentDTO.BookingID,
            Amount = createPaymentDTO.Amount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        return await _paymentRepository.CreatePaymentAsync(newPayment);
    }
}