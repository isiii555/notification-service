using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Persistence.Context;

namespace BackgroundTask.Application.Services
{
    /// <summary>
    /// This service manages the notification sending process by utilizing appropriate 
    /// message providers and communication channels. In case of failure, it logs 
    /// unsent notifications for future retries.
    /// </summary>

    public class NotificationService(
        IMessageProviderFactory messageProviderFactory,
        IChannelFactory channelFactory,
        NotificationContext context) : INotificationService
    {
        private readonly IChannelFactory _channelFactory = channelFactory;
        private readonly NotificationContext _context = context;
        private readonly IMessageProviderFactory _messageProviderFactory = messageProviderFactory;

        public async Task<bool> SendNotificationAsync(Notification notification)
        {
            var relevantProviders = _messageProviderFactory.GetRelevantProviders(notification.Channel);

            var channel = _channelFactory.GetChannel(notification.Channel);

            var result = await channel!.SendNotificationAsync(notification, relevantProviders!);

            if (!result)
            {
                if (!_context.Notifications.Contains(notification))
                {
                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();
                }
                return false;
            }

            return true;
        }
    }
}
