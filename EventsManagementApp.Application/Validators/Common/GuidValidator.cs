using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class GuidValidator : AbstractValidator<string>
{
    public GuidValidator(string parameterName)
    {
        RuleFor(x => x)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage($"{parameterName} must be a valid GUID");
    }
}