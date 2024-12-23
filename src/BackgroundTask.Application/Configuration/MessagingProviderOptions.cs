using BackgroundTask.Domain.Models;

namespace BackgroundTask.Application.Configuration
{
    /// <summary>
    /// Represents configuration options for messaging providers.
    /// </summary>
    /// <remarks>
    /// This class holds a collection of provider configurations mapped by their names.
    /// Each provider's configuration is represented by the <see cref="ProviderConfiguration"/> class.
    /// </remarks>

    public class MessagingProviderOptions
    {
        public Dictionary<string, ProviderConfiguration> Providers { get; set; }
    }
}
