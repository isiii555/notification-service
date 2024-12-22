using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Application.Interfaces
{
    public interface IChannel
    {
        Task<bool> SendNotificationAsync(Notification notification, IEnumerable<IMessageProvider> providers);

        NotificationChannel ChannelName { get; }
    }
}
