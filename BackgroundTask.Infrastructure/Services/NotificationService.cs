using BackgroundTask.Application.Configuration;
using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Persistence.Context;
using Microsoft.Extensions.Options;

namespace BackgroundTask.Infrastructure.Services
{
    public class NotificationService(
        IEnumerable<IMessageProvider> providers,
        IEnumerable<IChannel> channels,
        IOptions<MessagingProviderOptions> options,
        NotificationContext context) : INotificationService
    {
        private readonly IEnumerable<IChannel> _channels = channels;
        private readonly MessagingProviderOptions _options = options.Value;
        private readonly NotificationContext _context = context;
        private readonly IEnumerable<IMessageProvider> _providers = providers;

        public async Task<bool> SendNotificationAsync(Notification notification)
        {
            var relevantProviders = _options.Providers
                .Where(p => p.Value.Enabled && p.Value!.Channels!.Contains(notification.Channel))
                .OrderBy(p => p.Value.Priority)
                .Select(p => _providers.FirstOrDefault(pr => pr.ProviderName == p.Key))
                .Where(p => p != null)
                .ToList();

            var channel = _channels.Where(c => c.ChannelName == notification.Channel).FirstOrDefault();

            var result = await channel!.SendNotificationAsync(notification, relevantProviders!);

            if (!result && !_context.Notifications.Contains(notification))
            {
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                return false;
            }

            return true;
        }
    }
}
