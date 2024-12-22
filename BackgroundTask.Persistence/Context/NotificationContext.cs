using BackgroundTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackgroundTask.Persistence.Context
{
    public class NotificationContext : DbContext
    {
        public DbSet<Notification> Notifications { get; set; }

        public NotificationContext(DbContextOptions<NotificationContext> options)
            : base(options)
        {

        }
    }
}
