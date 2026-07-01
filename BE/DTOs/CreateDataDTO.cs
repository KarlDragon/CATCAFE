using System.ComponentModel.DataAnnotations.Schema;

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