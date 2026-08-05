namespace BE.DTOs;
public class UpdateCatDTO
{
    public int CatID {get; set;}
    public string? CatName {get; set;}
    public string? Breed {get; set;}
    public string? Status {get; set;}
    public string? Description {get; set;}
}