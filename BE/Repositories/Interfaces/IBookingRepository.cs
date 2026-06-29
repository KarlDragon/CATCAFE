namespace BE.Repositories.Interfaces;
using BE.Models;
public interface IBookingRepository
{
    Task<bool> CreateBookingAsync( Booking booking );

    Task<bool> ChangeBookingStatusAsync( int bookingId );

    Task<IEnumerable<Booking>> GetBookingsAsync();
}