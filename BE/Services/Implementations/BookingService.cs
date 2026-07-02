namespace BE.Services.Implementations;
using BE.DTOs;
using BE.Models;
using BE.Services.Interfaces;
using BE.Repositories.Interfaces;
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFoodDrinkRepository _foodDrinkRepository;
    public BookingService(IBookingRepository bookingRepository, IFoodDrinkRepository foodDrinkRepository)
    {
        _bookingRepository = bookingRepository;
        _foodDrinkRepository = foodDrinkRepository;
    }

    public async Task CreateBookingAsync( CreateBookingDTO createBookingDTO )
    {
        var foodDrinkIds = createBookingDTO.BookingDetails.Select(d => d.FoodDrinkID).Distinct().ToList();
        var foodDrinkPrices = await _foodDrinkRepository.GetFoodDrinkPriceByIdsAsync(foodDrinkIds);

        var newBooking = new Booking
        {
            TableID = createBookingDTO.TableID,
            UserID = createBookingDTO.UserID,
            BookedTime = createBookingDTO.BookedTime,
            EndTime = createBookingDTO.EndTime,
            Status = BookingStatus.Pending,

            BookingCats = createBookingDTO.BookingCats.Select( c => new BookingCat { CatID = c.CatID }).ToList(),
            BookingDetails = createBookingDTO.BookingDetails.Select( d => new BookingDetail { 
                                                            FoodDrinkID = d.FoodDrinkID, 
                                                            Quantity = d.Quantity, 
                                                            PriceAtBooking = foodDrinkPrices.GetValueOrDefault(d.FoodDrinkID) }).ToList()

        };

        var result = await _bookingRepository.CreateBookingAsync(newBooking);

        if (!result)
        {
            throw new Exception("Failed to create booking.");
        }
    }

    public async Task ChangeBookingStatusAsync( int bookingId, BookingStatus bookingStatus )
    {
        var result = await _bookingRepository.ChangeBookingStatusAsync(bookingId, bookingStatus);

        if (!result)
        {
            throw new Exception($"Failed to change booking status for booking ID {bookingId}.");
        }

    }

    public async Task<IEnumerable<Booking>> GetBookingsAsync()
    {
        return await _bookingRepository.GetBookingsAsync();
    }
}