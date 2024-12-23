using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BackgroundTask.API.Controllers;
using BackgroundTask.Application.Interfaces;
using BackgroundTask.Application.DTOs;
using BackgroundTask.Domain.Entities;
using BackgroundTask.API.Constants;
using Mapster;
using BackgroundTask.Domain.Enums;

namespace BackgroundTask.API.Test.Controllers
{
    public class NotificationControllerTests
    {
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly NotificationController _notificationController;

        public NotificationControllerTests()
        {
            _notificationServiceMock = new Mock<INotificationService>();
            _notificationController = new NotificationController(_notificationServiceMock.Object);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldReturnOk_WhenNotificationIsSentSuccessfully()
        {
            // Arrange
            var sendNotificationRequest = new SendNotificationRequest
            {
                Recipient = "test@example.com",
                Message = "Test Message",
                Channel = NotificationChannel.Email
            };
            var notification = sendNotificationRequest.Adapt<Notification>();

            _notificationServiceMock
                .Setup(service => service.SendNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(true);

            // Act
            var result = await _notificationController.SendNotificationAsync(sendNotificationRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(NotificationMessages.NotificationSentSuccessfully, okResult.Value);
        }

        [Fact]
        public async Task SendNotificationAsync_ShouldReturnInternalServerError_WhenNotificationIsNotSentSuccessfully()
        {
            // Arrange
            var sendNotificationRequest = new SendNotificationRequest
            {
                Recipient = "test@example.com",
                Message = "Test Message",
                Channel = NotificationChannel.Email
            };
            var notification = sendNotificationRequest.Adapt<Notification>();

            _notificationServiceMock
                .Setup(service => service.SendNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(false);

            // Act
            var result = await _notificationController.SendNotificationAsync(sendNotificationRequest);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal(NotificationMessages.NotificationFailed, statusCodeResult.Value);
        }
    }
}
