using BackgroundTask.Domain.Entities;
using BackgroundTask.Application.Interfaces;

namespace BackgroundTask.Infrastructure.Providers
{
    public class TwilioMessageProvider : IMessageProvider
    {
        public string ProviderName => "Twilio";

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
