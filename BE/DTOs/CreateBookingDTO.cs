namespace BE.DTOs;
public class CreateBookingDTO
{
    public int TableID { get; set; }
    public DateTime BookedTime { get; set; } 
    public DateTime EndTime { get; set; }
    public ICollection<CreateBookingCatDTO> BookingCats { get; set; } = new List<CreateBookingCatDTO>();
    public ICollection<CreateBookingDetailDTO> BookingDetails { get; set; } = new List<CreateBookingDetailDTO>();
}

