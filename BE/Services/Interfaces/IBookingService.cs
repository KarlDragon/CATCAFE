namespace BE.Services.Interfaces;
using BE.Models;
public interface IBookingService
{
    Task<bool> CreateBookingAsync( Booking booking );

    Task<bool> ChangeBookingStatusAsync( int bookingId, BookingStatus bookingStatus );

    Task<IEnumerable<Booking>> GetBookingsAsync();
}