using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Enums;
using BackgroundTask.Infrastructure.Services;
using Moq;
using Xunit;

namespace BackgroundTask.Infrastructure.Test.Services
{
    public class ChannelFactoryTests
    {
        private readonly List<IChannel> _channels;
        private readonly ChannelFactory _channelFactory;

        public ChannelFactoryTests()
        {
            var emailChannelMock = new Mock<IChannel>();
            emailChannelMock.Setup(c => c.ChannelName).Returns(NotificationChannel.Email);

            var smsChannelMock = new Mock<IChannel>();
            smsChannelMock.Setup(c => c.ChannelName).Returns(NotificationChannel.Sms);

            _channels = new List<IChannel>
            {
                emailChannelMock.Object,
                smsChannelMock.Object
            };

            _channelFactory = new ChannelFactory(_channels);
        }

        [Fact]
        public void GetChannel_ShouldReturnCorrectChannel_WhenChannelExists()
        {
            // Act
            var emailChannel = _channelFactory.GetChannel(NotificationChannel.Email);
            var smsChannel = _channelFactory.GetChannel(NotificationChannel.Sms);

            // Assert
            Assert.NotNull(emailChannel);
            Assert.Equal(NotificationChannel.Email, emailChannel.ChannelName);

            Assert.NotNull(smsChannel);
            Assert.Equal(NotificationChannel.Sms, smsChannel.ChannelName);
        }

        [Fact]
        public void GetChannel_ShouldReturnNull_WhenChannelDoesNotExist()
        {
            // Act
            var pushChannel = _channelFactory.GetChannel(NotificationChannel.Push);

            // Assert
            Assert.Null(pushChannel);
        }
    }
}
