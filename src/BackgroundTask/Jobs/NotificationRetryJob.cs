using BackgroundTask.Application.Interfaces;
using BackgroundTask.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BackgroundTask.API.Jobs
{
    /// <summary>
    /// Background service that retries sending unsent notifications.
    /// </summary>
    /// <remarks>
    /// The <see cref="NotificationRetryJob"/> is a background service that processes unsent notifications from the database and attempts to resend them at regular intervals. It uses scoped services to interact with the notification context and notification service.
    /// </remarks>

    public class NotificationRetryJob(IServiceScopeFactory scopeFactory) : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using AsyncServiceScope scope = await ProcessingAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"{ex.Message}");
            }
        }

        private async Task<AsyncServiceScope> ProcessingAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateAsyncScope();
            using var _context = scope.ServiceProvider.GetRequiredService<NotificationContext>();
            var _notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            while (!stoppingToken.IsCancellationRequested)
            {
                var unsentNotifications = await _context.Notifications
                    .ToListAsync(stoppingToken);

                foreach (var notification in unsentNotifications)
                {
                    Console.WriteLine($"Retrying to send notification to {notification.Recipient}...");
                    if (await _notificationService.SendNotificationAsync(notification))
                    {
                        _context.Notifications.Remove(notification);
                        await _context.SaveChangesAsync(stoppingToken);
                        Console.WriteLine($"Notification sent successfully on retry.");
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            return scope;
        }
    }
}
