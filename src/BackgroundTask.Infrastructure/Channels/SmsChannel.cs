using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Infrastructure.Channels
{
    /// <summary>
    /// Represents the SMS channel for sending notifications.
    /// </summary>
    /// <remarks>
    /// The <see cref="SmsChannel"/> is responsible for sending notifications via SMS using
    /// the available message providers. It attempts to send the notification through each provider
    /// until one succeeds, marking the notification as sent.
    /// </remarks>
    
    public class SmsChannel : IChannel
    {
        public NotificationChannel ChannelName => NotificationChannel.Sms;

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
