using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Infrastructure.Services
{
    /// <summary>
    /// Factory for retrieving communication channels based on the notification channel type.
    /// </summary>
    /// <remarks>
    /// The <see cref="ChannelFactory"/> is responsible for providing an appropriate implementation 
    /// of <see cref="IChannel"/> based on the specified notification channel.
    /// </remarks>

    public class ChannelFactory(IEnumerable<IChannel> channels) : IChannelFactory
    {
        private readonly IEnumerable<IChannel> _channels = channels;

        public IChannel? GetChannel(NotificationChannel channelName)
        {
            return _channels.FirstOrDefault(c => c.ChannelName == channelName);
        }
    }
}
