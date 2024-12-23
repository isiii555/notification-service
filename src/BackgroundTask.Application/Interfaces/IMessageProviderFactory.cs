using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Application.Interfaces
{
    public interface IMessageProviderFactory
    {
        List<IMessageProvider?> GetRelevantProviders(NotificationChannel channel);
    }
}
