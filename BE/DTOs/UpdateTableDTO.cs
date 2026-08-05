namespace BE.DTOs;
public class UpdateTableDTO
{
    public int TableID { get; set; }
    public string? TableName { get; set; }
    public int? SeatAmount { get; set; }
}