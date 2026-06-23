using System.ComponentModel.DataAnnotations;

namespace BE.Models;

public class Cat
{
    [Key]
    public int CatID {get; set;}
    public string CatName {get; set;} = "";
    public string Breed {get; set;} = "";
    public string Status {get; set;} = "";
}