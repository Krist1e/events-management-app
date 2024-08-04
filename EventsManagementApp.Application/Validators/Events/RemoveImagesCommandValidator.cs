using EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class RemoveImagesCommandValidator : AbstractValidator<RemoveImagesCommand>
{
    public RemoveImagesCommandValidator()
    {
        RuleForEach(x => x.ImageIds)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("ImageId must be a valid GUID.");
    }
}