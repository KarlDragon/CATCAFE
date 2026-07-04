using System.ComponentModel.DataAnnotations;

namespace BE.Models;

public class Table
{
    [Key]
    public int TableID {get; set;}
    public string TableName {get; set;} = "";
    public int SeatAmount {get; set;}
    public bool IsActive { get; set; } = true;
}