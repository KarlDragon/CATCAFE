namespace BE.DTOs;

public class UpdateFoodDrinkDTO
{
    public int FoodDrinkID {get; set;}
    public string? Name {get; set;}
    public decimal? Price {get; set;}
    public int? Quantity {get; set;}
}