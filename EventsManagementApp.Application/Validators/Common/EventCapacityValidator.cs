using FluentValidation;

namespace EventsManagementApp.Application.Validators.Common;

public class EventCapacityValidator : AbstractValidator<int>
{
    private const int MinCapacity = 0;

    public EventCapacityValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Event capacity is required.")
            .GreaterThan(MinCapacity)
            .WithMessage($"Event capacity must be greater than {MinCapacity}.");
    }
}