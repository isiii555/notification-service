using BackgroundTask.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundTask.Persistence
{
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
