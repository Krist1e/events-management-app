using EventsManagementApp.Application.UseCases.Users.Queries.GetUserById;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required")
            .Must(x => Guid.TryParse(x, out _))
            .WithMessage("UserId must be a valid GUID.");
    }
}