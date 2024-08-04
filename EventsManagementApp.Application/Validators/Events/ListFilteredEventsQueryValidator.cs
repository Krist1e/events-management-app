using EventsManagementApp.Application.UseCases.Events.Queries.ListFilteredEvents;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class ListFilteredEventsQueryValidator : AbstractValidator<ListFilteredEventsQuery>
{
    public ListFilteredEventsQueryValidator()
    {
        RuleFor(x => x.QueryParameters)
            .SetValidator(new EventQueryParametersValidator());
    }
}