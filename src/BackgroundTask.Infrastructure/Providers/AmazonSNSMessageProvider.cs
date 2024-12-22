using BackgroundTask.Domain.Entities;
using BackgroundTask.Application.Interfaces;

namespace BackgroundTask.Infrastructure.Providers
{
    public class AmazonSNSMessageProvider : IMessageProvider
    {
        public string ProviderName => "AmazonSNS";

        public async Task<bool> SendNotificationAsync(Notification notification)
        {
            try
            {
                //implementation of provider...
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
