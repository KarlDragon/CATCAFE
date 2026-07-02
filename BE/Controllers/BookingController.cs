namespace BE.Controllers;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        await _bookingService.CreateBookingAsync(createBookingDTO, userId);
        return Ok(new { message = "Booking created successfully." });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllBookings(CancellationToken cancellationToken)
    {
        var bookings = await _bookingService.GetAllBookingsAsync(cancellationToken);
        return Ok(bookings);
    }
}