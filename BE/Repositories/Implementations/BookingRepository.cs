namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }
}