using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Application.Interfaces
{
    public interface IChannelFactory
    {
        IChannel? GetChannel(NotificationChannel channelName);
    }
}
