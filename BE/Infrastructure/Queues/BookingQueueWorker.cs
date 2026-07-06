namespace BE.Infrastructure.Queue;

using BE.Models;
using BE.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class BookingQueueWorker : BackgroundService
{
    private readonly IRequestQueue<BookingQueueRequest> _queue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BookingQueueWorker> _logger;

    public BookingQueueWorker(
        IRequestQueue<BookingQueueRequest> queue,
        IServiceProvider serviceProvider,
        ILogger<BookingQueueWorker> logger)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Booking queue worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            BookingQueueRequest request;
            try
            {
                request = await _queue.DequeueAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            using var scope = _serviceProvider.CreateScope();
            var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

            try
            {
                _logger.LogInformation("Processing queued booking for user {UserId}.", request.UserId);
                var result = await bookingService.CreateBookingInternalAsync(request.CreateBookingDTO, request.UserId);
                request.CompletionSource.SetResult(result);
                _logger.LogInformation("Queued booking processed for user {UserId}.", request.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process queued booking for user {UserId}.", request.UserId);
                // swallow exceptions so worker continues processing next items

                // Set the exception on the TaskCompletionSource to propagate the error back to the caller
                request.CompletionSource.SetException(ex);
            }
        }

        _logger.LogInformation("Booking queue worker stopping.");
    }
}
