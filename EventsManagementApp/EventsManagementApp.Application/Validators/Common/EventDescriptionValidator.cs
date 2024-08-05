using FluentValidation;
using FluentValidation.Validators;

namespace EventsManagementApp.Application.Validators.Common;

public class EventDescriptionValidator : AbstractValidator<string>
{
    private const int DescriptionMaxLength = 500;

    public EventDescriptionValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Event description is required.")
            .MaximumLength(DescriptionMaxLength)
            .WithMessage($"Event description must not exceed {DescriptionMaxLength} characters.");
    }
}