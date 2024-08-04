using EventsManagementApp.Application.UseCases.Users.Commands.UnregisterFromEvent;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class UnregisterFromEventCommandValidator : AbstractValidator<UnregisterFromEventCommand>
{
    public UnregisterFromEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("EventId is required")
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("EventId must be a valid GUID.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("UserId must be a valid GUID.");
    }
}