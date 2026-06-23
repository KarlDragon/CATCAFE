using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.Models;

public class Table
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TableID {get; set;}
    public int? UserID {get; set;}
    public DateTime? BookedTime {get; set;}
}