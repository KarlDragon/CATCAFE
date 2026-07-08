namespace BE.Services.Implementations;
using BE.DTOs;
using BE.Models;
using BE.Services.Interfaces;
using BE.Repositories.Interfaces;
using BE.Exceptions;
using BE.Infrastructure.Queue;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFoodDrinkRepository _foodDrinkRepository;
    private readonly IRequestQueue<BookingQueueRequest> _bookingQueue;
    public BookingService(IBookingRepository bookingRepository, IFoodDrinkRepository foodDrinkRepository, IRequestQueue<BookingQueueRequest> bookingQueue)
    {
        _bookingRepository = bookingRepository;
        _foodDrinkRepository = foodDrinkRepository;
        _bookingQueue = bookingQueue;
    }

    public async Task<BookingResult> CreateBookingInternalAsync( CreateBookingDTO createBookingDTO, int userId )
    {
        var foodDrinkIds = createBookingDTO.BookingDetails.Select(d => d.FoodDrinkID).Distinct().ToList();
        var foodDrinkPrices = await _foodDrinkRepository.GetFoodDrinkPriceByIdsAsync(foodDrinkIds);

        // Validate all requested food/drink IDs exist
        var missingIds = foodDrinkIds.Where(id => !foodDrinkPrices.ContainsKey(id)).ToList();
        if (missingIds.Any())
        {
            throw new NotFoundException($"The following FoodDrink IDs were not found or are inactive: {string.Join(", ", missingIds)}");
        }

        var isDuplicate = await _bookingRepository.IsDuplicateBookingAsync(createBookingDTO.TableID, createBookingDTO.BookedTime, createBookingDTO.EndTime);
        if (isDuplicate)
        {
            throw new DuplicateBookingException("The booking time overlaps with an existing booking for the same table.");
        }
        var newBooking = new Booking
        {
            TableID = createBookingDTO.TableID,
            UserID = userId,
            BookedTime = createBookingDTO.BookedTime,
            EndTime = createBookingDTO.EndTime,
            Status = BookingStatus.Pending,

            BookingCats = [.. createBookingDTO.BookingCats.Select( c => new BookingCat { CatID = c.CatID })],
            BookingDetails = [.. createBookingDTO.BookingDetails.Select( d => new BookingDetail { 
                                                            FoodDrinkID = d.FoodDrinkID, 
                                                            Quantity = d.Quantity, 
                                                            PriceAtBooking = foodDrinkPrices[d.FoodDrinkID] })]

        };

        var result = await _bookingRepository.CreateBookingAsync(newBooking);

        if (result == null)
        {
            throw new FailedToCreateException("Failed to create booking.");
        }

        return new BookingResult
        {
            BookingId = result.BookingID,
            Status = result.Status
        };
    }

    public async Task<BookingResult> EnqueueBookingAsync( CreateBookingDTO createBookingDTO, int userId, CancellationToken cancellationToken )
    {
        var request = new BookingQueueRequest
        {
            CreateBookingDTO = createBookingDTO,
            UserId = userId,
            CompletionSource = new TaskCompletionSource<BookingResult>(
                TaskCreationOptions.RunContinuationsAsynchronously)
        };
        await _bookingQueue.EnqueueAsync(request, cancellationToken);

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        cts.Token.Register(() => request.CompletionSource.TrySetCanceled());

        return await request.CompletionSource.Task;
    }
    public async Task ChangeBookingStatusAsync( int bookingId, BookingStatus bookingStatus )
    {
        var result = await _bookingRepository.ChangeBookingStatusAsync(bookingId, bookingStatus);

        if (!result)
        {
            throw new NotFoundException($"Failed to change booking status for booking ID {bookingId}.");
        }

    }

    public async Task<IEnumerable<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken)
    {
        return await _bookingRepository.GetAllBookingsAsync(cancellationToken);
    }
}