using BackgroundTask.Application.Interfaces;
using BackgroundTask.Application.Services;
using BackgroundTask.Infrastructure.Channels;
using BackgroundTask.Infrastructure.Providers;
using BackgroundTask.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundTask.Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering infrastructure services in the dependency injection container.
    /// </summary>
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageProvider, TwilioMessageProvider>();
            services.AddScoped<IMessageProvider, AmazonSNSMessageProvider>();
            services.AddScoped<IMessageProviderFactory, MessageProviderFactory>();
            services.AddScoped<IChannel, EmailChannel>();
            services.AddScoped<IChannel, SmsChannel>();
            services.AddScoped<IChannelFactory, ChannelFactory>();
            return services;
        }
    }
}

