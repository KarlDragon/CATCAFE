namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Repositories.Interfaces;
using BE.DTOs;
using BE.Models;
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentAttempRepository _paymentAttempRepository;

    public PaymentService(IPaymentRepository paymentRepository,
                          IPaymentAttempRepository paymentAttempRepository )
    {
        _paymentRepository = paymentRepository;
        _paymentAttempRepository = paymentAttempRepository;
    }

    public async Task CreatePaymentAsync(CreatePaymentDTO createPaymentDTO)
    {
        var newPayment = new Payment
        {
            BookingID = createPaymentDTO.BookingID,
            Amount = createPaymentDTO.Amount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        int paymentID = await _paymentRepository.CreatePaymentAsync(newPayment);

        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss"); 
        string orderId = $"CATCAFE-{paymentID}-{timestamp}";
        string requestId = Guid.NewGuid().ToString();

        var newPaymentAttempt = new PaymentAttempt
        {
            PaymentID = paymentID,
            OrderId = orderId,
            RequestId = requestId,
            Status = PaymentAttemptStatus.Init,
            CreatedAt = DateTime.UtcNow
        };
        await _paymentAttempRepository.CreatePaymentAttemptAsync(newPaymentAttempt);
    }
}