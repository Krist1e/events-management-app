using EventsManagementApp.Application.UseCases.Users.Commands.RegisterInEvent;
using EventsManagementApp.Application.Validators.Events;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class RegisterInEventCommandValidator : AbstractValidator<RegisterInEventCommand>
{
    public RegisterInEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .SetValidator(x => new GuidValidator(nameof(x.EventId)));

        RuleFor(x => x.UserId)
            .SetValidator(x => new GuidValidator(nameof(x.UserId)));
    }
}