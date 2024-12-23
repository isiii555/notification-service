using BackgroundTask.Domain.Entities;
using BackgroundTask.Application.Interfaces;

namespace BackgroundTask.Infrastructure.Providers
{
    /// <summary>
    /// Implements a message provider for sending notifications via Amazon SNS (Simple Notification Service).
    /// </summary>
    /// <remarks>
    /// This provider sends notifications through Amazon SNS, allowing messages to be delivered 
    /// to the appropriate recipients via supported channels (e.g., SMS, Email).
    /// </remarks>

    public class AmazonSNSMessageProvider : IMessageProvider
    {
        public string ProviderName => "AmazonSNS";

        public async virtual Task<bool> SendNotificationAsync(Notification notification)
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
