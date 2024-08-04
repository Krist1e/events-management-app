using EventsManagementApp.Application.UseCases.Events.Commands.AddImages;
using EventsManagementApp.Application.Validators.Common;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class AddImagesCommandValidator : AbstractValidator<AddImagesCommand>
{
    public AddImagesCommandValidator()
    {
        RuleFor(x => x.EventId)
            .SetValidator(x => new GuidValidator(nameof(x.EventId)));
    }
}