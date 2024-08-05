using EventsManagementApp.Application.UseCases.Events.Commands.UpdateEvent;
using EventsManagementApp.Application.Validators.Common;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;


public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Event.Id)
            .SetValidator(x => new GuidValidator(nameof(x.Event.Id)));
        
        RuleFor(x => x.Event.Name)
            .SetValidator(new EventNameValidator());

        RuleFor(x => x.Event.Description)
            .SetValidator(new EventDescriptionValidator());

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