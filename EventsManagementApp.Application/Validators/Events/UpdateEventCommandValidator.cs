using EventsManagementApp.Application.UseCases.Events.Commands.UpdateEvent;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public static class EventRulesExtensions
{
    private const int NameMaxLength = 100;

    public static IRuleBuilder<T, string> EnsureNameNotEmpty<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .WithMessage("Name is required");

    public static IRuleBuilder<T, string> EnsureNameMaxLength<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .MaximumLength(NameMaxLength)
            .WithMessage("Name must not exceed 100 characters");
}

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    private const int DescMaxLength = 500;
    private const int LocationMaxLength = 100;
    private const int CategoryMaxLength = 50;
    private const int MinCapacity = 0;

    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Event.Id)
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("Id must be a valid GUID");

        /*RuleFor(x => x.Event.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(NameMaxLength)
            .WithMessage("Name must not exceed 100 characters");*/
        RuleFor(x => x.Event.Name).EnsureNameNotEmpty().EnsureNameMaxLength();

        RuleFor(x => x.Event.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(DescMaxLength)
            .WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Event.StartDate)
            .NotEmpty()
            .WithMessage("StartDate is required")
            .GreaterThanOrEqualTo(DateTime.Now)
            .WithMessage("StartDate must be greater than or equal to current date");

        RuleFor(x => x.Event.EndDate)
            .NotEmpty()
            .WithMessage("EndDate is required")
            .GreaterThan(x => x.Event.StartDate)
            .WithMessage("EndDate must be greater than StartDate");

        RuleFor(x => x.Event.Location)
            .NotEmpty()
            .WithMessage("Location is required")
            .MaximumLength(LocationMaxLength)
            .WithMessage("Location must not exceed 100 characters");

        RuleFor(x => x.Event.Category)
            .NotEmpty()
            .WithMessage("Category is required")
            .MaximumLength(CategoryMaxLength)
            .WithMessage("Category must not exceed 50 characters");

        RuleFor(x => x.Event.Capacity)
            .NotEmpty()
            .WithMessage("Capacity is required")
            .GreaterThan(MinCapacity)
            .WithMessage("Capacity must be greater than 0");
    }
}