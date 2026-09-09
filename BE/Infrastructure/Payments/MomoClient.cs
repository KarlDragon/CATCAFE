namespace BE.Infrastructure.Payments;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public class MomoClient
{
    private readonly IConfiguration _config;
    private readonly MomoSignature _momoSignature;
    private readonly IHttpClientFactory _httpClientFactory;
    public MomoClient(IConfiguration configuration, MomoSignature momoSignature, IHttpClientFactory httpClientFactory)
    {
        _config = configuration;
        _momoSignature = momoSignature;
        _httpClientFactory = httpClientFactory;
    }
    public async Task<GatewayPaymentResponse> InitiatePayment(GatewayPaymentRequest gatewayPaymentRequest, CancellationToken cancellationToken = default)
    {
        var momoCredentials = _config.GetSection("MomoCredentials");
        string partnerCode = momoCredentials["PartnerCode"] ?? throw new Exception("Check the partner code, maybe null?");
        string storeName = momoCredentials["StoreName"] ?? throw new Exception("Check the store name man");
        string ipnUrl = momoCredentials["IpnUrl"] ?? throw new Exception("Did you fill out the ipnUrl yet?");
        string requestType = momoCredentials["RequestType"] ?? throw new Exception("Check the requestType!");
        string lang = momoCredentials["Lang"] ?? throw new Exception(" vi or en or u forgot both?");
        string accessKey = momoCredentials["AccessKey"] ?? throw new Exception("This is important, CHECK CHECK CHECK because the accesskey is null!");
        string secretKey = momoCredentials["SecretKey"] ?? throw new Exception("Where's the secretKey of Momo???");
        string momoEndpoint = momoCredentials["MoMoApiEndpoint"] ?? throw new Exception("Re-check the momo endpoint");

        var reactClient = _config.GetSection("ReactClient"); 
        string redirectUrl = reactClient["RedirectUrlAfterPay"] ?? throw new Exception("Remember to check redirect link");
        
        string signature = _momoSignature.CreateAndHash_HmacSha256_Signature(accessKey,
                                                                gatewayPaymentRequest.Amount,
                                                                gatewayPaymentRequest.ExtraData,
                                                                ipnUrl,
                                                                gatewayPaymentRequest.OrderId,
                                                                gatewayPaymentRequest.OrderInfo,
                                                                partnerCode,
                                                                redirectUrl,
                                                                gatewayPaymentRequest.RequestId,
                                                                requestType,
                                                                secretKey);
        HttpClient client = _httpClientFactory.CreateClient();

        var momoRequest = new MomoRequest
        {
            PartnerCode = partnerCode,
            StoreName = storeName,
            RequestId = gatewayPaymentRequest.RequestId,
            Amount = gatewayPaymentRequest.Amount,
            OrderId = gatewayPaymentRequest.OrderId,
            OrderInfo = gatewayPaymentRequest.OrderInfo,
            RedirectUrl = redirectUrl,
            IpnUrl = ipnUrl,
            RequestType = requestType,
            ExtraData = gatewayPaymentRequest.ExtraData,
            Lang = lang,
            Signature = signature
        };

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage response = await client.PostAsJsonAsync(momoEndpoint, momoRequest, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            // Log body in case something went wrong
            throw new InvalidOperationException($"MoMo request failed ({(int)response.StatusCode}): {body}");
        }
        GatewayPaymentResponse momoResponse = JsonSerializer.Deserialize<GatewayPaymentResponse>(body)
            ?? throw new InvalidOperationException("MoMo response was empty.");
        return momoResponse;
    }
}