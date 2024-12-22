using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Application.DTOs
{
    public class SendNotificationRequest
    {
        public string Message { get; set; } = null!;
        public NotificationChannel Channel { get; set; }
        public string Recipient { get; set; } = null!;
    }
}
