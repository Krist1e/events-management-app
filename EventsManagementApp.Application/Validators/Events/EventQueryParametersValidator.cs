using EventsManagementApp.Application.UseCases.Events.Contracts;
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
        
        // todo: add validation for OrderBy string

        RuleFor(x => x.Name)
            .MaximumLength(NameMaxLength)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Location)
            .MaximumLength(LocationMaxLength)
            .WithMessage("Location must not exceed 100 characters");

        RuleFor(x => x.Category)
            .MaximumLength(CategoryMaxLength)
            .WithMessage("Category must not exceed 50 characters");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.Now)
            .WithMessage("Start date must be greater than or equal to current date");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be greater than StartDate");
    }
}