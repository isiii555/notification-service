using BackgroundTask.Application.Interfaces;
using BackgroundTask.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundTask.Application
{
    /// <summary>
    /// Provides extension methods for registering application services in the dependency injection container.
    /// </summary>
    public static  class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            return services;
        }
    }
}
