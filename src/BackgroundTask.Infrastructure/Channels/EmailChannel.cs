using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Infrastructure.Channels
{
    /// <summary>
    /// Represents the email channel for sending notifications.
    /// </summary>
    /// <remarks>
    /// The <see cref="EmailChannel"/> is responsible for sending notifications via email using
    /// the available message providers. It attempts to send the notification through each provider
    /// until one succeeds.
    /// </remarks>   
    public class EmailChannel : IChannel
    {
        public NotificationChannel ChannelName => NotificationChannel.Email;

        public async Task<bool> SendNotificationAsync(Notification notification, IEnumerable<IMessageProvider> providers)
        {
            foreach (var provider in providers)
            {
                if (await provider.SendNotificationAsync(notification))
                {
                    notification.SentAt = DateTime.UtcNow;
                    return true;
                }
            }
            return false;
        }
    }
}
