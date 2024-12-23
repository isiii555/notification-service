using BackgroundTask.Domain.Entities;
using BackgroundTask.Application.Interfaces;

namespace BackgroundTask.Infrastructure.Providers
{
    /// <summary>
    /// Implements a message provider for sending notifications via Twilio.
    /// </summary>
    /// <remarks>
    /// This provider integrates with the Twilio API to send notifications to recipients 
    /// via supported channels (e.g., SMS, Voice).
    /// </remarks>
    
    public class TwilioMessageProvider : IMessageProvider
    {
        public string ProviderName => "Twilio";

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
