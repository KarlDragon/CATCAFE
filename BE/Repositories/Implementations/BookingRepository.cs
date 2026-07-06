namespace BE.Repositories.Implementations;
using System;
using BE.Repositories.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> CreateBookingAsync( Booking booking )
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        if (booking.BookingCats == null) throw new ArgumentNullException(nameof(booking.BookingCats));
        if (booking.BookingDetails == null) throw new ArgumentNullException(nameof(booking.BookingDetails));

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<bool> ChangeBookingStatusAsync(int bookingId, BookingStatus bookingStatus)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return false;
        booking.Status = bookingStatus;
        var affected = await _context.SaveChangesAsync();
        return affected > 0;
    }

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken)
    {
        return await _context.Bookings
            .Include(b => b.BookingCats)
            .Include(b => b.BookingDetails)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateBookingAsync(int tableId, DateTime bookedTime, DateTime endTime)
    {
        return await _context.Bookings.AnyAsync(b => b.TableID == tableId &&
                                                     b.Status != BookingStatus.Cancelled &&
                                                     b.Status != BookingStatus.Completed && 
                                                     b.BookedTime < endTime && b.EndTime > bookedTime);
    }
}