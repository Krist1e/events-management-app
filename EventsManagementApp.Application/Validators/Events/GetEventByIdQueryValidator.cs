using EventsManagementApp.Application.UseCases.Events.Queries.GetEventById;
using EventsManagementApp.Application.Validators.Common;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .SetValidator(x => new GuidValidator(nameof(x.Id)));
    }
}