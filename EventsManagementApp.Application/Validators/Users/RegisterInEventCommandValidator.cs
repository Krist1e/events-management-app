using EventsManagementApp.Application.UseCases.Users.Commands.RegisterInEvent;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class RegisterInEventCommandValidator : AbstractValidator<RegisterInEventCommand>
{
    public RegisterInEventCommandValidator()
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