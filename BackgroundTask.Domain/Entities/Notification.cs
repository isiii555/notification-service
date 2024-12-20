using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Domain.Entities
{
    public class Notification
    {
        public string Recipient { get; set; } = null!;

        public string Message { get; set; } = null!;

        public NotificationChannel Channel { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? SentAt { get; set; }
    }
}
