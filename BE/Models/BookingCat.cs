using System.ComponentModel.DataAnnotations;

namespace BE.Models;

public class BookingCat
{
    public int BookingID { get; set; } 
    public int CatID { get; set; }  
    public Booking Booking { get; set; } = null!;
    public Cat Cat { get; set; } = null!;
}