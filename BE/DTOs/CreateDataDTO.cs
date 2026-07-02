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
    public DateTime BookedTime { get; set; } 
    public DateTime EndTime { get; set; }
    public ICollection<CreateBookingCatDTO> BookingCats { get; set; } = new List<CreateBookingCatDTO>();
    public ICollection<CreateBookingDetailDTO> BookingDetails { get; set; } = new List<CreateBookingDetailDTO>();
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