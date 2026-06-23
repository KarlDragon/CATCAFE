using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace BE.Models;

public class Booking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookingID { get; set; }
    public int TableID { get; set; }
    public int UserID { get; set; }   
    public DateTime BookedTime { get; set; } 
    public DateTime EndTime { get; set; }
    public enum BookingStatus { Pending, Confirmed, Cancelled, Completed }
    public BookingStatus Status { get; set; }

    // Navigation Property
    public Table? Table { get; set; }
    public Users? User { get; set; }
    public ICollection<BookingCat> BookingCats { get; set; } = new List<BookingCat>();
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}
