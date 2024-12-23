using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;
using BackgroundTask.Infrastructure.Providers;
using Xunit;

namespace BackgroundTask.Infrastructure.Test.Providers
{
    public class AmazonSNSMessageProviderTests
    {
        [Fact]
        public async Task SendNotificationAsync_ReturnsTrue_WhenNotificationIsValid()
        {
            // Arrange
            var validNotification = new Notification
            {
                Id = Guid.NewGuid(),
                Recipient = "recipient@example.com",
                Message = "This is a test message.",
                Channel = NotificationChannel.Email,
                CreatedAt = DateTime.UtcNow
            };
            var messageProvider = new TwilioMessageProvider();

            // Act
            var result = await messageProvider.SendNotificationAsync(validNotification);

            // Assert
            Assert.True(result);
        }


        [Fact]
        public async Task SendNotificationAsync_ReturnsFalse_WhenExceptionOccurs()
        {
            // Arrange
            var faultyNotification = new Notification
            {
                Id = Guid.NewGuid(),
                Recipient = "recipient@example.com",
                Message = "This message will cause an exception.",
                Channel = NotificationChannel.Email,
                CreatedAt = DateTime.UtcNow
            };

            var messageProvider = new FaultyTwilioMessageProvider();

            // Act
            var result = await messageProvider.SendNotificationAsync(faultyNotification);

            // Assert
            Assert.False(result);
        }
    }

    public class FaultyAmazonSNSMessageProvider : AmazonSNSMessageProvider
    {
        public override async Task<bool> SendNotificationAsync(Notification notification)
        {
            try
            {
                throw new InvalidOperationException("Simulated exception during message sending.");
            }
            catch
            {
                return false;
            }
        }
    }
}
