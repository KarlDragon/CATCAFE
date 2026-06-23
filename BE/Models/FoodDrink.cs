using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BE.Models;

public class FoodDrink
{
    [Key]
    public int FoodDrinkID {get; set;}
    public string Name {get; set;} = "";
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price {get; set;}
    public int Quantity {get; set;}
}