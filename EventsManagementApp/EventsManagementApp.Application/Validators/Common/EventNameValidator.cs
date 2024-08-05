using FluentValidation;

namespace EventsManagementApp.Application.Validators.Common;

public class EventNameValidator : AbstractValidator<string>
{
    private const int NameMaxLength = 100;
    
    public EventNameValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Event name is required.")
            .MaximumLength(NameMaxLength)
            .WithMessage($"Event name must not exceed {NameMaxLength} characters.");
    }
}