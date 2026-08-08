namespace BE.DTOs;
using BE.Models;
public class UpdatePaymentDTO
{
    public int PaymentID {get; set;}
    public PaymentStatus? PaymentStatus {get; set;}
    public DateTime? PaidAt {get; set;}
    public int? SuccessfulAttemptId {get; set;}
}