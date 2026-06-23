using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BE.Models;

public class BookingDetail
{
    public int BookingID { get; set; }  
    public int FoodDrinkID { get; set; } 
    public int Quantity { get; set; }   
    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceAtBooking { get; set; } 
    public Booking Booking { get; set; } = null!;
    public FoodDrink FoodDrink { get; set; } = null!;
}