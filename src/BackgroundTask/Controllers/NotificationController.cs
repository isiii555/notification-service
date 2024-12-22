using BackgroundTask.Application.DTOs;
using BackgroundTask.Application.Interfaces;
using BackgroundTask.Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundTask.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("/")]
        public async Task<IActionResult> SendNotificationAsync([FromBody] SendNotificationRequest sendNotificationRequest)
        {
            if (sendNotificationRequest == null || string.IsNullOrEmpty(sendNotificationRequest.Message) || string.IsNullOrEmpty(sendNotificationRequest.Recipient))
            {
                return BadRequest("Invalid notification data.");
            }

            var notification = sendNotificationRequest.Adapt<Notification>();

            var isSent = await _notificationService.SendNotificationAsync(notification);

            if (isSent)
            {
                return Ok("Notification sent successfully.");
            }

            return StatusCode(500, "Failed to send notification.");
        }
    }
}
