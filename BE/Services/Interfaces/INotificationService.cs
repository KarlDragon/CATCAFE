namespace BE.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string toUserName, string toEmail, string subject, string body, CancellationToken cancellationToken = default);
        Task SendWelcomeEmailAsync(string toUserName, string toEmail, CancellationToken cancellationToken = default);
        Task SendBookingConfirmationEmailAsync(int userId, int bookingId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);
    }
}