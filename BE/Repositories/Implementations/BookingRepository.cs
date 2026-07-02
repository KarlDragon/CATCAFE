namespace BE.Repositories.Implementations;
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

    public async Task<bool> CreateBookingAsync( Booking booking )
    {
        _context.Bookings.Add(booking);
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<bool> ChangeBookingStatusAsync(int bookingId, BookingStatus bookingStatus)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) return false;
        booking.Status = bookingStatus;
        var affected = await _context.SaveChangesAsync();
        return affected > 0;
    }

    public async Task<IEnumerable<Booking>> GetBookingsAsync()
    {
        return await _context.Bookings
            .Include(b => b.BookingCats)
            .Include(b => b.BookingDetails)
            .ToListAsync();
    }

}