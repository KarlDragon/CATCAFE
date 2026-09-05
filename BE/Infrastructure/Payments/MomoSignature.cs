using System.Security.Cryptography;
using System.Text;

namespace BE.Infrastructure.Payments;

public class MomoSignature
{
    public string CreateAndHash_HmacSha256_Signature(string accessKey, long amount, string extraData,
                                            string ipnUrl, string orderId, string orderInfo, string partnerCode,
                                            string redirectUrl, string requestId, string requestType, string secretKey)
    {
        var rawSignature = $"accessKey={accessKey}" + $"&amount={amount}" + $"&extraData={extraData}" +
            $"&ipnUrl={ipnUrl}" + $"&orderId={orderId}" + $"&orderInfo={orderInfo}" +
            $"&partnerCode={partnerCode}" + $"&redirectUrl={redirectUrl}" + 
            $"&requestId={requestId}" + $"&requestType={requestType}";
        
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var signatureBytes = Encoding.UTF8.GetBytes(rawSignature);

        var hashBytes = new HMACSHA256(keyBytes).ComputeHash(signatureBytes);

        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}