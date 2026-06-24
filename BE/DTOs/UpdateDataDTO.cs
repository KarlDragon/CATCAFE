namespace BE.DTOs;

public class UpdateTableDTO
{
    public int TableID { get; set; }
    public string? TableName { get; set; } = "";
    public int? SeatAmount { get; set; }
}

public class UpdateCatDTO
{
    public int CatID {get; set;}
    public string? CatName {get; set;} = "";
    public string? Breed {get; set;} = "";
    public string? Status {get; set;} = "";
    
}

public class UpdateFoodDrinkDTO
{
    public int FoodDrinkID {get; set;}
    public string? Name {get; set;} = "";
    public decimal? Price {get; set;}
    public int? Quantity {get; set;}
}