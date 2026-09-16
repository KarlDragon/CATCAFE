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
    private readonly IMomoClient _momoClient;
    private readonly IPaymentGatewayLogRepository _paymentGatewayLogRepository;
    public PaymentService(IPaymentRepository paymentRepository,
                          IPaymentAttempRepository paymentAttempRepository,
                          IBookingService bookingService,
                          IMomoClient momoClient,
                          IPaymentGatewayLogRepository paymentGatewayLogRepository)
    {
        _paymentRepository = paymentRepository;
        _paymentAttempRepository = paymentAttempRepository;
        _bookingService = bookingService;
        _momoClient = momoClient;
        _paymentGatewayLogRepository = paymentGatewayLogRepository;
    }

    public async Task<string> CreatePaymentAsync(CreatePaymentDTO createPaymentDTO, CancellationToken cancellationToken = default)
    {
        Payment? payment = await _paymentRepository.GetPaymentWithBookingId(createPaymentDTO.BookingID);
        if ( payment != null)
        {
            throw new FailedToCreateException($"BookingId {createPaymentDTO.BookingID} already has a payment");
        }
        int amount = await _bookingService.CalculateTotalBookingPriceAsync(createPaymentDTO.BookingID);

        int paymentID = await CreatePendingPaymentAsync(createPaymentDTO.BookingID, amount);

        PaymentAttempt attempt = await CreatePaymentAttemptAsync(paymentID);

        string orderInfo = await _bookingService.GetTableInfo(createPaymentDTO.BookingID);
        var gatewayRequest = BuildGatewayRequest(attempt, amount, orderInfo);

        return await InitiateGatewayPaymentAsync(attempt, gatewayRequest, cancellationToken);
    }

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

    private async Task<string> InitiateGatewayPaymentAsync(PaymentAttempt attempt, GatewayPaymentRequest gatewayPaymentRequest, CancellationToken cancellationToken)
    {
        int? resultCode = null;

        try
        {
            var (momoRequest, rawPayload) = _momoClient.BuildPaymentRequest(gatewayPaymentRequest);

            await CreatePaymentLog(new PaymentGatewayLogDTO
            {
                PaymentID = attempt.PaymentID,
                RawPayload = rawPayload,
                Direction = PaymentLogDirection.CreateRequest
            });

            var momoResponse = await _momoClient.SendPaymentRequest(momoRequest, cancellationToken);
            resultCode = momoResponse.ResultCode;

            if (momoResponse.ResultCode != 0)
            {
                throw new FailedToCreateException(
                    $"MoMo payment initiation failed: {momoResponse.Message}");
            }

            await _paymentAttempRepository.UpdatePaymentAttemptAsync(
            new UpdatePaymentAttemptDTO
            {
                AttemptId = attempt.AttemptID,
                Status = PaymentAttemptStatus.Redirected,
                PayUrl = momoResponse.PayUrl,
                ResultCode = momoResponse.ResultCode
            });

            return momoResponse.PayUrl;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            await MarkAttemptFailedAsync(attempt.AttemptID, resultCode);

            throw;
        }
    }

    private async Task MarkAttemptFailedAsync(int attemptId, int? resultCode = null)
    {
        await _paymentAttempRepository.UpdatePaymentAttemptAsync(
            new UpdatePaymentAttemptDTO
            {
                AttemptId = attemptId,
                Status = PaymentAttemptStatus.Failed,
                ResultCode = resultCode
            });
    }

    private async Task<bool> CreatePaymentLog(PaymentGatewayLogDTO paymentGatewayLogDTO)
    {
        var paymentLog = new PaymentGatewayLog
        {
            PaymentID = paymentGatewayLogDTO.PaymentID,
            RawPayload = paymentGatewayLogDTO.RawPayload,
            Direction = paymentGatewayLogDTO.Direction,
            CreatedAt = DateTime.UtcNow
        };
        return await _paymentGatewayLogRepository.CreatePaymentGatewayLogAsync(paymentLog);
    }

}