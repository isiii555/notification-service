using BackgroundTask.Application.DTOs;
using FluentValidation;

namespace BackgroundTask.Application.Validators
{
    /// <summary>
    /// Validator for <see cref="SendNotificationRequest"/>.
    /// Ensures that the required fields of the notification request, such as 
    /// the message and recipient, are provided and not empty.
    /// </summary>
 
    public class SendNotificationRequestValidator : AbstractValidator<SendNotificationRequest>
    {
        public SendNotificationRequestValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .WithMessage("Message is required.");

            RuleFor(x => x.Recipient)
                .NotEmpty()
                .WithMessage("Recipient is required.");
        }
    }
}
