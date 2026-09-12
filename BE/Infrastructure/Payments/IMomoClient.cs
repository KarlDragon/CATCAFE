namespace BE.Infrastructure.Payments;
public interface IMomoClient
{
    (MomoRequest Request, string RawPayload) BuildPaymentRequest(GatewayPaymentRequest gatewayPaymentRequest);
    Task<GatewayPaymentResponse> SendPaymentRequest(MomoRequest momoRequest, CancellationToken cancellationToken = default);
}