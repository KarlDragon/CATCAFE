namespace BE.Infrastructure.Queue;

using BE.Models;
using BE.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
public class BookingQueueWorker : BackgroundService
{
    private readonly IRequestQueue<BookingQueueRequest> _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BookingQueueWorker> _logger;

    public BookingQueueWorker(
        IRequestQueue<BookingQueueRequest> queue,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<BookingQueueWorker> logger)
    {
        _queue = queue;
        _scopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Booking queue worker started.");

        await foreach (var request in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

            try
            {
                _logger.LogInformation("Processing queued booking for user {UserId}.", request.UserId);
                var result = await bookingService.CreateBookingInternalAsync(request.CreateBookingDTO, request.UserId);
                request.CompletionSource.TrySetResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process queued booking for user {UserId}.", request.UserId);
                request.CompletionSource.TrySetException(ex);
            }
        }

        _logger.LogInformation("Booking queue worker stopping.");
    }
}
