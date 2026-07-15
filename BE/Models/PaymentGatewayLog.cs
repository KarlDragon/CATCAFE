namespace BE.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum PaymentLogDirection {CreateRequest, CreateResponse, IPN, QueryRequest, QueryResponse}

public class PaymentGatewayLog
{
    [Key]
    public int PaymentGatewayLogID { get; set; }

    public int PaymentID { get; set; }
    [ForeignKey(nameof(PaymentID))]
    public Payment? Payment { get; set; }

    public PaymentLogDirection Direction { get; set; }
    public required string RawPayload { get; set; } 

    public DateTime CreatedAt { get; set; }
}