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