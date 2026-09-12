namespace BE.DTOs;
using BE.Models;

public class PaymentGatewayLogDTO{
    public int PaymentID { get; set; }
    public PaymentLogDirection Direction { get; set; }
    public required string RawPayload { get; set; } 
}