namespace BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
public interface IBookingService
{
    Task<BookingResult> CreateBookingInternalAsync( CreateBookingDTO createBookingDTO, int userId );

    Task<BookingResult> EnqueueBookingAsync( CreateBookingDTO createBookingDTO, int userId, CancellationToken cancellationToken );

    Task ChangeBookingStatusAsync( int bookingId, BookingStatus bookingStatus );

    Task<IEnumerable<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken);
}