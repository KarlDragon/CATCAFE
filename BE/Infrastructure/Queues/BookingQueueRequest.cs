namespace BE.Infrastructure.Queue;

using BE.DTOs;

public class BookingQueueRequest
{
    public CreateBookingDTO CreateBookingDTO { get; set; } = null!;
    public int UserId { get; set; }
}
