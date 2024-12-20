using BackgroundTask.Domain.Enums;

namespace BackgroundTask.Domain.Models
{
    public class ProviderConfiguration
    {
        public bool Enabled { get; set; }
        public int Priority { get; set; }
        public string ApiKey { get; set; }
        public List<NotificationChannel> Channels { get; set; } = new();
    }
}
