namespace BE.Models;

public class NotificationQueueRequest
{
    public string ToUserName { get; set; } = string.Empty;
    public string ToEmail { get; set; } = string.Empty;
}