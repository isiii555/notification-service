using BackgroundTask.Domain.Models;

namespace BackgroundTask.Application.Configuration
{
    public class MessagingProviderOptions
    {
        public Dictionary<string, ProviderConfiguration> Providers { get; set; } = new();
    }
}
