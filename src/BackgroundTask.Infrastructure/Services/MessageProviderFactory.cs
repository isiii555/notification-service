using BackgroundTask.Application.Configuration;
using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Enums;
using Microsoft.Extensions.Options;

namespace BackgroundTask.Infrastructure.Services
{
    /// <summary>
    /// Factory for retrieving relevant message providers based on the notification channel configuration.
    /// </summary>
    /// <remarks>
    /// The <see cref="MessageProviderFactory"/> is responsible for selecting the appropriate message providers 
    /// for a given notification channel based on the settings defined in the <see cref="MessagingProviderOptions"/>.
    /// </remarks>

    public class MessageProviderFactory(IEnumerable<IMessageProvider> providers, IOptionsMonitor<MessagingProviderOptions> optionsMonitor) : IMessageProviderFactory
    {
        private readonly IEnumerable<IMessageProvider> _providers = providers;
        private readonly IOptionsMonitor<MessagingProviderOptions> _optionsMonitor = optionsMonitor;


        public List<IMessageProvider?> GetRelevantProviders(NotificationChannel channel)
        {
            var options = _optionsMonitor.CurrentValue;
            return options.Providers
                .Where(p => p.Value.Enabled && p.Value.Channels!.Contains(channel))
                .OrderBy(p => p.Value.Priority)
                .Select(p => _providers.FirstOrDefault(pr => pr.ProviderName == p.Key))
                .Where(p => p != null)
                .ToList();
        }
    }
}
