using FluentValidation;

namespace EventsManagementApp.Application.Validators.Common;

public class EventStartDateValidator : AbstractValidator<DateTime>
{
    public EventStartDateValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Event start date is required.")
            .GreaterThanOrEqualTo(DateTime.Now)
            .WithMessage("Event start date must be greater than or equal to the current date.");
    }
}