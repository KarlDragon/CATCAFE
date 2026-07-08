namespace BE.Infrastructure.Queue;

using BE.Models;
using BE.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BE.Repositories.Interfaces;
public class BookingQueueWorker : BackgroundService
{
    private readonly IRequestQueue<BookingQueueRequest> _queue;
    private readonly IRequestQueue<MailJob> _mailQueue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<BookingQueueWorker> _logger;

    public BookingQueueWorker(
        IRequestQueue<BookingQueueRequest> queue,
        IRequestQueue<MailJob> mailQueue,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<BookingQueueWorker> logger)
    {
        _queue = queue;
        _mailQueue = mailQueue;
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
            var authRepository = scope.ServiceProvider.GetRequiredService<IAuthRepository>();

            try
            {
                _logger.LogInformation("Processing queued booking for user {UserId}.", request.UserId);
                var result = await bookingService.CreateBookingInternalAsync(request.CreateBookingDTO, request.UserId);
                request.CompletionSource.TrySetResult(result);

                try
                {
                    var user = await authRepository.GetUserByIdAsync(request.UserId);
                    if (user != null)
                    {
                        var mailJob = new MailJob("BookingConfirmation", (svc, ct) =>
                            svc.SendBookingConfirmationEmailAsync(user.Username, user.Email, result.BookingId,
                                request.CreateBookingDTO.BookedTime, request.CreateBookingDTO.EndTime, ct));
                        await _mailQueue.EnqueueAsync(mailJob, CancellationToken.None);
                    }
                }
                catch (Exception mailEx)
                {
                    _logger.LogError(mailEx, "Failed to enqueue booking confirmation email for user {UserId}.", request.UserId);
                }
            
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
