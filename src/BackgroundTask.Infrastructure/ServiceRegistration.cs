using BackgroundTask.Application.Interfaces;
using BackgroundTask.Infrastructure.Channels;
using BackgroundTask.Infrastructure.Providers;
using BackgroundTask.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundTask.Infrastructure
{
    public static class ServiceRegistration
    {

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMessageProvider, TwilioMessageProvider>();
            services.AddScoped<IMessageProvider, AmazonSNSMessageProvider>();
            services.AddScoped<IChannel, EmailChannel>();
            services.AddScoped<IChannel, SmsChannel>();
            services.AddScoped<INotificationService, NotificationService>();
            return services;
        }
    }
}

