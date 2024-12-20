using BackgroundTask.Domain.Entities;

namespace BackgroundTask.Application.IServices
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(Notification notification);
    }
}
