using EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;
using EventsManagementApp.Application.Validators.Common;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class ListEventsByUserIdQueryValidator : AbstractValidator<ListEventsByUserIdQuery>
{
    public ListEventsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .SetValidator(x => new GuidValidator(nameof(x.UserId)));

        RuleFor(x => x.QueryParameters)
            .SetValidator(new QueryParametersValidator());
    }
}