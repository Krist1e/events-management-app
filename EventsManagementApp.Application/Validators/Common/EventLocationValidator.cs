using FluentValidation;

namespace EventsManagementApp.Application.Validators.Common;

public class EventLocationValidator : AbstractValidator<string>
{
    private const int LocationMaxLength = 100;
    
    public EventLocationValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Event location is required.")
            .MaximumLength(LocationMaxLength)
            .WithMessage($"Event location must not exceed {LocationMaxLength} characters.");
    }
}