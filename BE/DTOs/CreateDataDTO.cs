using System.ComponentModel.DataAnnotations.Schema;
using BE.Models;
namespace BE.DTOs;

public class CreateTableDTO
{
    public string TableName {get; set;} = "";
    public int SeatAmount {get; set;}
}

public class CreateCatDTO
{
    public string CatName {get; set;} = "";
    public string Breed {get; set;} = "";
    public string Status {get; set;} = "";
    public string Description {get; set;} = "";

}

public class CreateFoodDrinkDTO
{
    public string Name {get; set;} = "";
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price {get; set;}
    public int Quantity {get; set;}
}

public class CreateBookingDTO
{
    public int TableID { get; set; }
    public int UserID { get; set; }   
    public DateTime BookedTime { get; set; } 
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; }
    public ICollection<BookingCat> BookingCats { get; set; } = new List<BookingCat>();
    public ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();
}

public class CreateBookingCatDTO
{
    public int CatID { get; set; }
}

public class CreateBookingDetailDTO
{
    public int FoodDrinkID { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtBooking { get; set; }
}