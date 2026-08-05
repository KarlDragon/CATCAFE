using BE.Models;
namespace BE.DTOs;
public class CreatePaymentDTO
{
    public int BookingID { get; set; }
    public long Amount { get; set; }
}