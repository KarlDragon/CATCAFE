namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Repositories.Interfaces;
using BE.DTOs;
using BE.Models;
using BE.Infrastructure.Payments;
using BE.Exceptions;

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

    //create payment, first paymentAttempt and return payurl
    public async Task<string> CreatePaymentAsync(CreatePaymentDTO createPaymentDTO, CancellationToken cancellationToken = default)
    {
        int amount = await _bookingService.CalculateTotalBookingPriceAsync(createPaymentDTO.BookingID);

        int paymentID = await CreatePendingPaymentAsync(createPaymentDTO.BookingID, amount);

        PaymentAttempt attempt = await CreatePaymentAttemptAsync(paymentID);

        string orderInfo = await _bookingService.GetTableInfo(createPaymentDTO.BookingID);
        var gatewayRequest = BuildGatewayRequest(attempt, amount, orderInfo);

        return await InitiateGatewayPaymentAsync(attempt, gatewayRequest, cancellationToken);
    }

    //return paymentId
    private async Task<int> CreatePendingPaymentAsync(int bookingId, int amount)
    {
        
        var payment = new Payment
        {
            BookingID = bookingId,
            Amount = amount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        return await _paymentRepository.CreatePaymentAsync(payment);
    }

    // save attemp into db
    private async Task<PaymentAttempt> CreatePaymentAttemptAsync(int paymentId)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var attempt = new PaymentAttempt
        {
            PaymentID = paymentId,
            OrderId = $"CATCAFE-{paymentId}-{timestamp}",
            RequestId = Guid.NewGuid().ToString(),
            Status = PaymentAttemptStatus.Init,
            CreatedAt = DateTime.UtcNow
        };
        attempt.AttemptID = await _paymentAttempRepository.CreatePaymentAttemptAsync(attempt);
        return attempt;
    }

    private static GatewayPaymentRequest BuildGatewayRequest(PaymentAttempt attempt, int amount, string orderInfo)
    {
        return new GatewayPaymentRequest
        {
            RequestId = attempt.RequestId,
            Amount = amount,
            OrderId = attempt.OrderId,
            OrderInfo = orderInfo,
            ExtraData = ""
        };
    }

    private async Task<string> InitiateGatewayPaymentAsync(
        PaymentAttempt attempt, GatewayPaymentRequest gatewayRequest, CancellationToken cancellationToken)
    {
        try
        {
            GatewayPaymentResponse response = await _momoClient.InitiatePayment(gatewayRequest, cancellationToken);

            var updatePaymentAttempt = new UpdatePaymentAttemptDTO
            {
                AttemptId = attempt.AttemptID,
                Status = PaymentAttemptStatus.Redirected,
                PayUrl = response.PayUrl
            };
            await _paymentAttempRepository.UpdatePaymentAttemptAsync(updatePaymentAttempt);
            return response.PayUrl;
        }
        catch (Exception)
        {
            var updatePaymentAttempt = new UpdatePaymentAttemptDTO
            {
                Status = PaymentAttemptStatus.Failed
            };
            await _paymentAttempRepository.UpdatePaymentAttemptAsync(updatePaymentAttempt);
            throw;
        }
    }
}