namespace BE.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

public class CreateFoodDrinkDTO
{
    public string Name {get; set;} = "";
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price {get; set;}
    public int Quantity {get; set;}
}