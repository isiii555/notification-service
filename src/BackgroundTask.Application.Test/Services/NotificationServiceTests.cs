using BackgroundTask.Application.Interfaces;
using BackgroundTask.Application.Services;
using BackgroundTask.Domain.Entities;
using BackgroundTask.Domain.Enums;
using BackgroundTask.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BackgroundTask.Application.Test.Services
{
    public class NotificationServiceTests
    {
        private readonly Mock<IMessageProviderFactory> _messageProviderFactoryMock;
        private readonly Mock<IChannelFactory> _channelFactoryMock;
        private readonly NotificationContext _context;
        private readonly NotificationService _notificationService;

        public NotificationServiceTests()
        {
            _messageProviderFactoryMock = new Mock<IMessageProviderFactory>();
            _channelFactoryMock = new Mock<IChannelFactory>();

            var options = new DbContextOptionsBuilder<NotificationContext>()
                .UseInMemoryDatabase(databaseName: "NotificationTestDb")
                .Options;

            _context = new NotificationContext(options);

            _notificationService = new NotificationService(
                _messageProviderFactoryMock.Object,
                _channelFactoryMock.Object,
                _context);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldReturnTrue_WhenNotificationIsSentSuccessfully()
        {
            // Arrange
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Recipient = "test@example.com",
                Message = "Test Message",
                Channel = NotificationChannel.Email,
                CreatedAt = DateTime.UtcNow
            };
            var providers = new List<IMessageProvider?> { new Mock<IMessageProvider>().Object };
            var channelMock = new Mock<IChannel>();

            _messageProviderFactoryMock
                .Setup(m => m.GetRelevantProviders(notification.Channel))
                .Returns(providers);

            _channelFactoryMock
                .Setup(c => c.GetChannel(notification.Channel))
                .Returns(channelMock.Object);

            channelMock
                .Setup(c => c.SendNotificationAsync(notification, providers!))
                .ReturnsAsync(true);

            //Act
            var result = await _notificationService.SendNotificationAsync(notification);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldReturnFalseAndSaveNotification_WhenNotificationIsNotSentSuccessfully()
        {
            //Arrange
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Recipient = "test@example.com",
                Message = "Test Message",
                Channel = NotificationChannel.Email,
                CreatedAt = DateTime.UtcNow
            };
            var providers = new List<IMessageProvider?> { new Mock<IMessageProvider>().Object };
            var channelMock = new Mock<IChannel>();

            _messageProviderFactoryMock
                .Setup(m => m.GetRelevantProviders(notification.Channel))
                .Returns(providers);

            _channelFactoryMock
                .Setup(c => c.GetChannel(notification.Channel))
                .Returns(channelMock.Object);

            channelMock
                .Setup(c => c.SendNotificationAsync(notification, providers!))
                .ReturnsAsync(false);
            
            //Act
            var result = await _notificationService.SendNotificationAsync(notification);

            //Assert
            Assert.False(result);

            var savedNotification = await _context.Notifications
                .SingleOrDefaultAsync(n => n.Id == notification.Id);
            Assert.NotNull(savedNotification);
        }
    }
}
