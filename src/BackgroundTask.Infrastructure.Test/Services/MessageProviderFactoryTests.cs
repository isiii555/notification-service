using BackgroundTask.Application.Configuration;
using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Enums;
using BackgroundTask.Domain.Models;
using BackgroundTask.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BackgroundTask.Infrastructure.Test.Services
{
    public class MessageProviderFactoryTests
    {
        private readonly Mock<IOptionsMonitor<MessagingProviderOptions>> _optionsMonitorMock;
        private readonly List<IMessageProvider> _providers;
        private readonly MessageProviderFactory _messageProviderFactory;

        public MessageProviderFactoryTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<MessagingProviderOptions>>();
            _providers = new List<IMessageProvider>
            {
                new Mock<IMessageProvider>().Object,
                new Mock<IMessageProvider>().Object
            };

            _messageProviderFactory = new MessageProviderFactory(_providers, _optionsMonitorMock.Object);
        }

        [Fact]
        public void GetRelevantProviders_ShouldReturnEnabledProviders_WithCorrectChannel()
        {
            // Arrange
            var options = new MessagingProviderOptions
            {
                Providers = new Dictionary<string, ProviderConfiguration>
                {
                    {
                        "Provider1",
                        new ProviderConfiguration
                        {
                            Enabled = true,
                            Channels = new List<NotificationChannel> { NotificationChannel.Email },
                            Priority = 1
                        }
                    },
                    {
                        "Provider2",
                        new ProviderConfiguration
                        {
                            Enabled = true,
                            Channels = new List<NotificationChannel> { NotificationChannel.Sms, NotificationChannel.Email },
                            Priority = 2
                        }
                    },
                    {
                        "Provider3",
                        new ProviderConfiguration
                        {
                            Enabled = false,
                            Channels = new List<NotificationChannel> { NotificationChannel.Email },
                            Priority = 3
                        }
                    }
                }
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

            var providerMocks = _providers.Select(p => Mock.Get(p)).ToList();
            providerMocks[0].Setup(p => p.ProviderName).Returns("Provider1");
            providerMocks[1].Setup(p => p.ProviderName).Returns("Provider2");

            // Act
            var relevantProviders = _messageProviderFactory.GetRelevantProviders(NotificationChannel.Email);

            // Assert
            Assert.NotNull(relevantProviders);
            Assert.Equal(2, relevantProviders.Count);
            Assert.Contains(relevantProviders, p => p.ProviderName == "Provider1");
            Assert.Contains(relevantProviders, p => p.ProviderName == "Provider2");
            Assert.DoesNotContain(relevantProviders, p => p.ProviderName == "Provider3");
        }

        [Fact]
        public void GetRelevantProviders_ShouldReturnProvidersInOrderOfPriority()
        {
            // Arrange
            var options = new MessagingProviderOptions
            {
                Providers = new Dictionary<string, ProviderConfiguration>
                {
                    {
                        "Provider1",
                        new ProviderConfiguration
                        {
                            Enabled = true,
                            Channels = new List<NotificationChannel> { NotificationChannel.Email },
                            Priority = 2
                        }
                    },
                    {
                        "Provider2",
                        new ProviderConfiguration
                        {
                            Enabled = true,
                            Channels = new List<NotificationChannel> { NotificationChannel.Email },
                            Priority = 1
                        }
                    }
                }
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(options);

            var providerMocks = _providers.Select(p => Mock.Get(p)).ToList();
            providerMocks[0].Setup(p => p.ProviderName).Returns("Provider1");
            providerMocks[1].Setup(p => p.ProviderName).Returns("Provider2");

            // Act
            var relevantProviders = _messageProviderFactory.GetRelevantProviders(NotificationChannel.Email);

            // Assert
            Assert.NotNull(relevantProviders);
            Assert.Equal(2, relevantProviders.Count);
            Assert.Equal("Provider2", relevantProviders[0]!.ProviderName);
            Assert.Equal("Provider1", relevantProviders[1]!.ProviderName);
        }
    }
}
