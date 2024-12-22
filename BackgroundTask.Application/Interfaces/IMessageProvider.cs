using BackgroundTask.Domain.Entities;

namespace BackgroundTask.Application.Interfaces
{
    public interface IMessageProvider
    {
        Task<bool> SendNotificationAsync(Notification notification);

        string ProviderName { get; }
    }
}
