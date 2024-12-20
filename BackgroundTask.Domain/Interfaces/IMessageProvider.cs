using BackgroundTask.Domain.Entities;

namespace BackgroundTask.Domain.Interfaces
{
    public interface IMessageProvider
    {
        Task<bool> SendNotificationAsync(Notification notification);

        string ProviderName { get; }
    }
}
