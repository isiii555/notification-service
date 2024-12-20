using BackgroundTask.Application.Configuration;
using BackgroundTask.Application.IServices;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace BackgroundTask.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEnumerable<IMessageProvider> _providers;
        private readonly MessagingProviderOptions _options;

        public NotificationService(
            IEnumerable<IMessageProvider> providers,
            IOptions<MessagingProviderOptions> options)
        {
            _providers = providers;
            _options = options.Value;
        }

        public async Task<bool> SendNotificationAsync(Notification notification)
        {
            var enabledProviders = _options.Providers
                .Where(p => p.Value.Enabled)
                .OrderBy(p => p.Value.Priority)
                .Select(p => _providers.FirstOrDefault(pr => pr.ProviderName == p.Key))
                .Where(p => p != null)
                .ToList();

            foreach (var provider in enabledProviders)
            {
                var providerConfig = _options.Providers[provider.ProviderName];
                if (!providerConfig.Channels.Contains(notification.Channel)) continue;

                if (await provider.SendNotificationAsync(notification))
                {
                    notification.SentAt = DateTime.UtcNow;
                    return true;
                }
            }
            //ScheduleRetry(notification);
            return false;
        }

    }
}
