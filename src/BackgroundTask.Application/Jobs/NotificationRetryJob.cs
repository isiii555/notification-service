using BackgroundTask.Application.Interfaces;
using BackgroundTask.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackgroundTask.Application.Jobs
{
    public class NotificationRetryJob : BackgroundService
    {
        private NotificationContext _context;
        private INotificationService _notificationService;
        private readonly IServiceScopeFactory _scopeFactory;

        public NotificationRetryJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();
            _context = scope.ServiceProvider.GetRequiredService<NotificationContext>();
            _notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
            while (!stoppingToken.IsCancellationRequested)
            {
                var unsentNotifications = await _context.Notifications
                    .ToListAsync(stoppingToken);

                foreach (var notification in unsentNotifications)
                {
                    Console.WriteLine($"Retrying notification to {notification.Recipient}...");
                    if (await _notificationService.SendNotificationAsync(notification))
                    {
                        _context.Notifications.Remove(notification);
                        await _context.SaveChangesAsync(stoppingToken);
                        Console.WriteLine($"Notification sent successfully on retry.");
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}
