namespace BE.Controllers;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BE.Exceptions;
using BE.Infrastructure.Queue;

[ApiController]
[Route("api/[controller]")]

public class BookingController : ControllerBase
{
    private readonly IRequestQueue<BookingQueueRequest> _bookingQueue;
    private readonly IBookingService _bookingService;

    public BookingController(
        IRequestQueue<BookingQueueRequest> bookingQueue,
        IBookingService bookingService)
    {
        _bookingQueue = bookingQueue;
        _bookingService = bookingService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDTO)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NotFoundException("User ID not found"));
        var request = new BookingQueueRequest
        {
            CreateBookingDTO = createBookingDTO,
            UserId = userId
        };
        await _bookingQueue.EnqueueAsync(request);
        return Ok(new { message = "Booking queued successfully." });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllBookings(CancellationToken cancellationToken)
    {
        var bookings = await _bookingService.GetAllBookingsAsync(cancellationToken);
        return Ok(bookings);
    }
}