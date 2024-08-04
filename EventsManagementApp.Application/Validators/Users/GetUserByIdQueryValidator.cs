using EventsManagementApp.Application.UseCases.Users.Queries.GetUserById;
using EventsManagementApp.Application.Validators.Common;
using EventsManagementApp.Application.Validators.Events;
using FluentValidation;

namespace EventsManagementApp.Application.Validators.Users;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .SetValidator(x => new GuidValidator(nameof(x.UserId)));
    }
}