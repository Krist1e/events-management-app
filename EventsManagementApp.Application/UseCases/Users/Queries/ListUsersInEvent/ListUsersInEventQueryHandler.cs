using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Users.Queries.ListUsersInEvent;

public class ListUsersInEventQueryHandler : IRequestHandler<ListUsersInEventQuery, IEnumerable<UserResponse>>
{
    private readonly IUserRepository _userRepository;

    public ListUsersInEventQueryHandler(IEventRepository eventRepository, IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponse>> Handle(ListUsersInEventQuery request,
        CancellationToken cancellationToken)
    {
        var eventId = Guid.Parse(request.EventId);
        var users = await _userRepository.GetUsersByEventIdAsync(eventId, cancellationToken);

        return users.Select(user =>
            new UserResponse(user.Id.ToString(), user.Email!, user.FirstName, user.LastName, user.DateOfBirth));
    }
}