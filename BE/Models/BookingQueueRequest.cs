namespace BE.Models;
using BE.DTOs;

public class BookingQueueRequest
{
    public CreateBookingDTO CreateBookingDTO { get; set; } = null!;
    public int UserId { get; set; }
    public TaskCompletionSource<bool> CompletionSource { get; set; } = new TaskCompletionSource<bool>();
}
