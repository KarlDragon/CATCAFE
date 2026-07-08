namespace BE.Controllers;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BE.Exceptions;

[ApiController]
[Route("api/[controller]")]

public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDTO createBookingDTO)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NotFoundException("User ID not found"));
        var bookingResult = await _bookingService.EnqueueBookingAsync(createBookingDTO, userId, CancellationToken.None);
        return Ok(new { message = "Booking queued successfully.", bookingId = bookingResult.BookingId, status = bookingResult.Status });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllBookings(CancellationToken cancellationToken)
    {
        var bookings = await _bookingService.GetAllBookingsAsync(cancellationToken);
        return Ok(bookings);
    }
}