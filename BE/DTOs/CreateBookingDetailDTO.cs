namespace BE.DTOs;
public class CreateBookingDetailDTO
{
    public int FoodDrinkID { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtBooking { get; set; }
}