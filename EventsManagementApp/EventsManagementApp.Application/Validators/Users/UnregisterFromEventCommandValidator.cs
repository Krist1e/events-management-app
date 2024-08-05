using EventsManagementApp.Application.UseCases.Users.Commands.UnregisterFromEvent;
using EventsManagementApp.Application.Validators.Common;
using EventsManagementApp.Application.Validators.Events;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class UnregisterFromEventCommandValidator : AbstractValidator<UnregisterFromEventCommand>
{
    public UnregisterFromEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .SetValidator(x => new GuidValidator(nameof(x.EventId)));

        RuleFor(x => x.UserId)
            .SetValidator(x => new GuidValidator(nameof(x.UserId)));
    }
}