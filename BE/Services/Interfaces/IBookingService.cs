namespace BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
public interface IBookingService
{
    Task CreateBookingAsync( CreateBookingDTO createBookingDTO, int userId );

    Task ChangeBookingStatusAsync( int bookingId, BookingStatus bookingStatus );

    Task<IEnumerable<Booking>> GetBookingsAsync();
}