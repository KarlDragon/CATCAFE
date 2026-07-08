using BE.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BE.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService( IConfiguration configuration, ILogger<NotificationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toUserName, string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        var emailSettings = _configuration.GetSection("MailSettings");
        var shopName = emailSettings["ShopName"] ?? throw new InvalidOperationException("ShopName is not configured in MailSettings.");
        var smtpServer = emailSettings["SmtpServer"] ?? throw new InvalidOperationException("SmtpServer is not configured in MailSettings.");
        var smtpPort = emailSettings["SmtpPort"] ?? throw new InvalidOperationException("SmtpPort is not configured in MailSettings.");
        if (!int.TryParse(smtpPort, out var smtpPortInt)) throw new FormatException("SmtpPort must be an integer");
        var smtpUsername = emailSettings["SmtpUsername"]  ?? throw new InvalidOperationException("SmtpUsername is not configured in MailSettings.");
        var smtpPassword = emailSettings["SmtpPassword"] ?? throw new InvalidOperationException("SmtpPassword is not configured in MailSettings.");
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(shopName, smtpUsername));
        message.To.Add(new MailboxAddress(toUserName, toEmail));
        message.Subject = subject;

        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(smtpServer, smtpPortInt, SecureSocketOptions.Auto, cancellationToken);
            _logger.LogInformation("Connected to SMTP server {SmtpServer}:{SmtpPort}", smtpServer, smtpPortInt);
            await client.AuthenticateAsync(smtpUsername, smtpPassword, cancellationToken);
            _logger.LogInformation("Authenticated with SMTP server {SmtpServer}", smtpServer);
            await client.SendAsync(message, cancellationToken   );
            _logger.LogInformation("Email sent successfully to {ToEmail}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
            throw; // Rethrow the exception after logging it
        }
        finally
        {
            if (client.IsConnected)  await client.DisconnectAsync(true, cancellationToken);
        }
    }
    
    public async Task SendWelcomeEmailAsync(string toUserName, string toEmail, CancellationToken cancellationToken = default)
    {
        var subject = "Chào mừng đến với Cat Cafe!";
        var body = $@"
        Xin chào {toUserName},

        Cảm ơn bạn đã đăng ký tài khoản tại Cat Cafe. 
        Chúng tôi rất vui mừng được chào đón bạn đến với cộng đồng của chúng tôi.
        Chúc bạn có những trải nghiệm tuyệt vời tại Cat Cafe!
        
        Trân trọng,
        Đội ngũ Cat Cafe
        ";

        await SendEmailAsync(toUserName, toEmail, subject, body, cancellationToken);
    }

    public async Task SendBookingConfirmationEmailAsync(string toUserName, 
    string toEmail, int bookingId, DateTime bookedTime, 
    DateTime endTime, CancellationToken cancellationToken = default)
    {
        var subject = "Xác nhận đặt bàn tại Cat Cafe";
        var body = $@"
        Xin chào {toUserName},

        Cảm ơn bạn đã đặt bàn tại Cat Cafe. 
        Thông tin đặt bàn của bạn như sau:
        
        Mã đặt bàn: {bookingId}
        Thời gian bắt đầu: {bookedTime:dd/MM/yyyy HH:mm}
        Thời gian kết thúc: {endTime:dd/MM/yyyy HH:mm}
        
        Chúng tôi mong được phục vụ bạn tại Cat Cafe!
        
        Trân trọng,
        Đội ngũ Cat Cafe
        ";

        await SendEmailAsync(toUserName, toEmail, subject, body, cancellationToken);
    }

    public async Task SendBookingStatusUpdateEmailAsync(string toUserName, string toEmail, int bookingId, string newStatus, CancellationToken cancellationToken = default)
    {
        var subject = string.Empty;
        var body = string.Empty;
        switch (newStatus.ToLower())
        {
            case "confirmed":
                subject = "Đã xác nhận thanh toán đơn hàng tại Cat Cafe";
                body = $@"
                Xin chào {toUserName},
                Cat Cafe xin thông báo rằng đơn hàng {bookingId} đã được xác nhận thanh toán thành công.
                Chúng tôi mong được phục vụ bạn tại Cat Cafe!
                Trân trọng,
                Đội ngũ Cat Cafe
                ";
                break;
            case "cancelled":
                subject = "Đã hủy đơn hàng tại Cat Cafe";
                body = $@"
                Xin chào {toUserName},
                Cat Cafe xin thông báo rằng đơn hàng {bookingId} đã được hủy.
                Chúng tôi xin lỗi vì sự bất tiện này.
                Trân trọng,
                Đội ngũ Cat Cafe
                ";
                break;
            case "completed":
                subject = "Cảm ơn bạn đã sử dụng dịch vụ tại Cat Cafe";
                body = $@"
                Xin chào {toUserName},
                Mã đơn hàng {bookingId}
                Đơn hàng của bạn đã được hoàn tất.

                Cảm ơn bạn đã sử dụng dịch vụ của Cat Cafe!
                Trân trọng,
                Đội ngũ Cat Cafe
                ";
                break;
            default:
                subject = "Cập nhật trạng thái đơn hàng tại Cat Cafe";
                body = $@"
                Xin chào {toUserName},
                Cat Cafe xin thông báo rằng trạng thái đơn hàng của bạn đã được cập nhật.
                Thông tin đơn hàng của bạn như sau:
                
                Mã đơn hàng: {bookingId}
                Trạng thái mới: {newStatus}
                
                Cảm ơn bạn đã sử dụng dịch vụ của Cat Cafe!
                
                Trân trọng,
                Đội ngũ Cat Cafe
                ";
                break;
        }

        await SendEmailAsync(toUserName, toEmail, subject, body, cancellationToken);
    }
}