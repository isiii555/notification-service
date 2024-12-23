using BackgroundTask.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundTask.Persistence
{
    /// <summary>
    /// Provides extension methods for registering persistence services in the dependency injection container.
    /// </summary>
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<NotificationContext>
                (options => options.UseInMemoryDatabase("NotificationDb"));
            return services;
        }
    }
}
