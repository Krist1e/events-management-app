using EventsManagementApp.Application.UseCases.Users.Queries.ListUsersInEvent;
using EventsManagementApp.Application.Validators.Common;
using EventsManagementApp.Application.Validators.Events;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class ListUsersInEventQueryValidator : AbstractValidator<ListUsersInEventQuery>
{
    public ListUsersInEventQueryValidator()
    {
        RuleFor(x => x.EventId)
            .SetValidator(x => new GuidValidator(nameof(x.EventId)));
    }
}