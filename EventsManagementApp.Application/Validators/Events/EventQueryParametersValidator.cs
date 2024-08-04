using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using EventsManagementApp.Application.Validators.Common;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Events;

public class EventQueryParametersValidator : AbstractValidator<EventQueryParameters>
{
    private const int NameMaxLength = 100;
    private const int LocationMaxLength = 100;
    private const int CategoryMaxLength = 50;

    public EventQueryParametersValidator()
    {
        Include(new QueryParametersValidator());

        RuleFor(x => x.Name)
            .MaximumLength(NameMaxLength)
            .WithMessage($"Name must not exceed {NameMaxLength} characters");

        RuleFor(x => x.Location)
            .MaximumLength(LocationMaxLength)
            .WithMessage($"Location must not exceed {LocationMaxLength} characters");

        RuleFor(x => x.Category)
            .MaximumLength(CategoryMaxLength)
            .WithMessage($"Category must not exceed {CategoryMaxLength} characters")
            .Must(x => Enum.TryParse<CategoryEnum>(x, true, out _))
            .WithMessage("Category must be a valid category");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.Now)
            .WithMessage("Start date must be greater than or equal to current date");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be greater than start date");
    }
}