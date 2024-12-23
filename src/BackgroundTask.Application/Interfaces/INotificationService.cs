using BackgroundTask.Application.DTOs;
using BackgroundTask.Domain.Entities;

namespace BackgroundTask.Application.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(Notification notification);
    }
}
