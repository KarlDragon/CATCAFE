namespace BE.Models;
using BE.Services.Interfaces;
public class MailJob
{
    public string JobName { get; set; } = string.Empty;
    public Func<INotificationService, CancellationToken, Task> Action { get; set; } = null!;
    public TaskCompletionSource<bool> Completion { get; set; } = new TaskCompletionSource<bool>();

    public MailJob(string jobName, Func<INotificationService, CancellationToken, Task> action)
    {
        JobName = jobName;
        Action = action;
    }
}