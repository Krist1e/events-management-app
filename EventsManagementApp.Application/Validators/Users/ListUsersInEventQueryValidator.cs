using EventsManagementApp.Application.UseCases.Users.Queries.ListUsersInEvent;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class ListUsersInEventQueryValidator : AbstractValidator<ListUsersInEventQuery>
{
    public ListUsersInEventQueryValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("EventId is required")
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("EventId must be a valid GUID.");
    }
}