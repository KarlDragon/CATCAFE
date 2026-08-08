namespace BE.Repositories.Interfaces;
using System;
using BE.Models;
public interface IBookingRepository
{
    Task<Booking> CreateBookingAsync( Booking booking );

    Task<bool> ChangeBookingStatusAsync( int bookingId, BookingStatus bookingStatus );

    Task<IEnumerable<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken);

    Task<bool> IsDuplicateBookingAsync( int tableId, DateTime bookedTime, DateTime endTime );
    public Task<Decimal> CaculdateTotalBookingPriceAsync(int bookingId);
}