using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BE.Models;
public enum PaymentStatus { Pending, Paid, Failed, Expired }
public class Payment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PaymentID { get; set; }
    public int BookingID { get; set; }
    [ForeignKey(nameof(BookingID))]
    public Booking? Booking { get; set; }
    public long Amount { get; set; } // Amount to be paid, in VND
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; } 
    public ICollection<PaymentAttempt> Attempts { get; set; } = new List<PaymentAttempt>();
}