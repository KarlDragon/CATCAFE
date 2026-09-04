namespace BE.Infrastructure.Payments;
public class GatewayPaymentRequest
{
    public string RequestId { get; set; } = "";
    public long Amount { get; set; }
    public string OrderId { get; set; } ="";
    public string OrderInfo { get; set; } = "";
    public string RedirectUrl { get; set; } = "";
    public string ExtraData { get; set; } = "";
}