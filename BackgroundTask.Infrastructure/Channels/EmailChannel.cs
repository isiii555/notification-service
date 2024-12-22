﻿using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Infrastructure.Channels
{
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
