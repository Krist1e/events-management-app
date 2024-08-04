using EventsManagementApp.Application.UseCases.Events.Queries.GetEventById;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class GetEventByIdQueryValidator : AbstractValidator<GetEventByIdQuery>
{
    public GetEventByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("Id format is invalid");
    }
}