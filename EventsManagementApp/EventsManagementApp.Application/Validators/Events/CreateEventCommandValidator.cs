using EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;
using EventsManagementApp.Application.Validators.Common;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Event.Name)
            .SetValidator(new EventNameValidator());

        RuleFor(x => x.Event.Description)
            .SetValidator(new EventDescriptionValidator());

        RuleFor(x => x.Event.StartDate)
            .SetValidator(new EventStartDateValidator());

        RuleFor(x => x.Event.EndDate)
            .SetValidator(x => new EventEndDateValidator(x.Event.StartDate));

        RuleFor(x => x.Event.Location)
            .SetValidator(new EventLocationValidator());

        RuleFor(x => x.Event.Category)
            .SetValidator(new EventCategoryValidator());

        RuleFor(x => x.Event.Capacity)
            .SetValidator(new EventCapacityValidator());
    }
}