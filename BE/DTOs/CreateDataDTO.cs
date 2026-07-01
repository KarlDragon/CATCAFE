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