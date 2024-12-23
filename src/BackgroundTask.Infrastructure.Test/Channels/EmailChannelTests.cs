using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;
using BackgroundTask.Infrastructure.Channels;
using Moq;
using Xunit;

namespace BackgroundTask.Infrastructure.Test.Channels
{
    public class EmailChannelTests
    {
        private readonly EmailChannel _emailChannel;

        public EmailChannelTests()
        {
            _emailChannel = new EmailChannel();
        }

        [Fact]
        public void ChannelName_ShouldReturnEmail()
        {
            // Act
            var channelName = _emailChannel.ChannelName;

            // Assert
            Assert.Equal(NotificationChannel.Email, channelName);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldReturnTrue_WhenNotificationIsSentSuccessfully()
        {
            // Arrange
            var notification = new Notification
            {
                Recipient = "test@example.com",
                Message = "Test Message",
                Channel = NotificationChannel.Email,
                CreatedAt = DateTime.UtcNow
            };

            var mockProvider = new Mock<IMessageProvider>();
            mockProvider.Setup(p => p.SendNotificationAsync(It.IsAny<Notification>())).ReturnsAsync(true);

            var providers = new List<IMessageProvider> { mockProvider.Object };

            // Act
            var result = await _emailChannel.SendNotificationAsync(notification, providers);

            // Assert
            Assert.True(result);
            Assert.NotNull(notification.SentAt);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldReturnFalse_WhenNoProviderSendsNotificationSuccessfully()
        {
            // Arrange
            var notification = new Notification
            {
                Recipient = "test@example.com",
                Message = "Test Message",
                Channel = NotificationChannel.Email,
                CreatedAt = DateTime.UtcNow
            };

            var mockProvider = new Mock<IMessageProvider>();
            mockProvider.Setup(p => p.SendNotificationAsync(It.IsAny<Notification>())).ReturnsAsync(false);

            var providers = new List<IMessageProvider> { mockProvider.Object };

            // Act
            var result = await _emailChannel.SendNotificationAsync(notification, providers);

            // Assert
            Assert.False(result);
            Assert.Null(notification.SentAt);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldReturnTrue_WhenFirstProviderFailsButNextProviderSucceeds()
        {
            // Arrange
            var notification = new Notification
            {
                Recipient = "test@example.com",
                Message = "Test Message",
                Channel = NotificationChannel.Email,
                CreatedAt = DateTime.UtcNow
            };

            var mockProvider1 = new Mock<IMessageProvider>();
            mockProvider1.Setup(p => p.SendNotificationAsync(It.IsAny<Notification>())).ReturnsAsync(false);

            var mockProvider2 = new Mock<IMessageProvider>();
            mockProvider2.Setup(p => p.SendNotificationAsync(It.IsAny<Notification>())).ReturnsAsync(true);

            var providers = new List<IMessageProvider> { mockProvider1.Object, mockProvider2.Object };

            // Act
            var result = await _emailChannel.SendNotificationAsync(notification, providers);

            // Assert
            Assert.True(result);
            Assert.NotNull(notification.SentAt);
        }
    }
}
