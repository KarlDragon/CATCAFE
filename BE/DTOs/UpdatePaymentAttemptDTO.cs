namespace BE.DTOs;
using BE.Models;
public class UpdatePaymentAttemptDTO
{
    public int AttemptId {get; set;}
    public PaymentAttemptStatus? Status {get; set;}
    public string? TransactionId {get; set;}
    public int? ResultCode {get; set;}
    public string? PayUrl {get; set;}
}