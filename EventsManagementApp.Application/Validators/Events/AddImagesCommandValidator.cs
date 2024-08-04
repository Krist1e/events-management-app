using EventsManagementApp.Application.UseCases.Events.Commands.AddImages;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class AddImagesCommandValidator : AbstractValidator<AddImagesCommand>
{
    public AddImagesCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("EventId format is invalid");
    }
}