using BackgroundTask.Application.Configuration;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;
using BackgroundTask.Domain.Interfaces;
using BackgroundTask.Domain.Models;
using Microsoft.Extensions.Options;

namespace BackgroundTask.Infrastructure.Providers
{
    public class AmazonSNSMessageProvider : IMessageProvider
    {
        private readonly ProviderConfiguration _config;

        public string ProviderName => "AmazonSNS";

        public AmazonSNSMessageProvider(IOptions<MessagingProviderOptions> options)
        {
            _config = options.Value.Providers[ProviderName];
            if (!_config.Enabled)
            {
                throw new InvalidOperationException($"{ProviderName} is disabled.");
            }
        }

        public async Task<bool> SendNotificationAsync(Notification notification)
        {
            if (_config.Channels.Contains(notification.Channel))
            {
                if (notification.Channel == NotificationChannel.Sms)
                {
                    // Implementation of provider...
                    Console.WriteLine($"Sending SMS via {ProviderName}: {notification.Message}");
                }
                else if (notification.Channel == NotificationChannel.Email)
                {
                    // Implementation of provider...
                    Console.WriteLine($"Sending Email via {ProviderName}: {notification.Message}");
                }
                return true;
            }
            return false;
        }
    }
}
