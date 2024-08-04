using FluentValidation;

namespace EventsManagementApp.Application.Validators.Common;

public class EventEndDateValidator : AbstractValidator<DateTime>
{
    public EventEndDateValidator(DateTime startDate)
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Event end date is required.")
            .GreaterThan(startDate)
            .WithMessage("Event end date must be greater than the start date.");
    }
}