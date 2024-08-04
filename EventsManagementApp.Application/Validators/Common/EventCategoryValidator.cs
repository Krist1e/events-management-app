using EventManagementApp.Domain.Enums;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Common;

public class EventCategoryValidator : AbstractValidator<string>
{
    private const int CategoryMaxLength = 50;

    public EventCategoryValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Event category is required.")
            .MaximumLength(CategoryMaxLength)
            .WithMessage($"Event category must not exceed {CategoryMaxLength} characters.")
            .Must(x => Enum.TryParse<CategoryEnum>(x, true, out _))
            .WithMessage("Event category must be a valid category.");
    }
}