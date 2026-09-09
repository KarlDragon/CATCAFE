namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Repositories.Interfaces;
using BE.DTOs;
using BE.Models;
using BE.Infrastructure.Payments;
public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentAttempRepository _paymentAttempRepository;
    private readonly IBookingService _bookingService;
    private readonly MomoClient _momoClient;
    public PaymentService(IPaymentRepository paymentRepository,
                          IPaymentAttempRepository paymentAttempRepository,
                          IBookingService bookingService,
                          MomoClient momoClient)
    {
        _paymentRepository = paymentRepository;
        _paymentAttempRepository = paymentAttempRepository;
        _bookingService = bookingService;
        _momoClient = momoClient;
    }

    public async Task CreatePaymentAsync(CreatePaymentDTO createPaymentDTO)
    {
        int amount = await _bookingService.CalculateTotalBookingPriceAsync(createPaymentDTO.BookingID);
        var newPayment = new Payment
        {
            BookingID = createPaymentDTO.BookingID,
            Amount = amount,
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

        var newGatewayPaymenyRequest = new GatewayPaymentRequest
        {
            RequestId = requestId,
            Amount = amount,
            OrderId = orderId,
            OrderInfo = createPaymentDTO.OrderId,
            ExtraData = ""
        };
        
        await _paymentAttempRepository.CreatePaymentAttemptAsync(newPaymentAttempt);
        await _momoClient.InitiatePayment(newGatewayPaymenyRequest);
    }
}