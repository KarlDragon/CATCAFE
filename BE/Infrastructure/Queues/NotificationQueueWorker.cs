namespace BE.Infrastructure.Queue;

using BE.Models;
using BE.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class NotificationQueueWorker : BackgroundService
{
    private readonly IRequestQueue<MailJob> _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationQueueWorker> _logger;

    public NotificationQueueWorker(
        IRequestQueue<MailJob> queue,
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationQueueWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification queue worker started.");

        await foreach (var job in _queue.Reader.ReadAllAsync(stoppingToken))
        {
            using var scope = _scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider
                .GetRequiredService<INotificationService>();

            try
            {
                await job.Action(notificationService, stoppingToken);
                job.Completion.TrySetResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mail job {JobName} failed", job.JobName);
                job.Completion.TrySetException(ex);
            }
        }

        _logger.LogInformation("Notification queue worker stopping.");
    }
}
