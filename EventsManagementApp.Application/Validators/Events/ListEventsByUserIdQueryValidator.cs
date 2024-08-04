using EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class ListEventsByUserIdQueryValidator : AbstractValidator<ListEventsByUserIdQuery>
{
    public ListEventsByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("UserId format is invalid");

        RuleFor(x => x.QueryParameters)
            .SetValidator(new QueryParametersValidator());
    }
}