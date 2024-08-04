using EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class RemoveImagesCommandValidator : AbstractValidator<RemoveImagesCommand>
{
    public RemoveImagesCommandValidator()
    {
        RuleForEach(x => x.ImageIds)
            .SetValidator(new GuidValidator("ImageId"));
    }
}