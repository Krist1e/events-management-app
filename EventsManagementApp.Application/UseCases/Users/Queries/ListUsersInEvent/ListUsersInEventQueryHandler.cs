using AutoMapper;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Users.Queries.ListUsersInEvent;

public class ListUsersInEventQueryHandler : IRequestHandler<ListUsersInEventQuery, IEnumerable<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public ListUsersInEventQueryHandler(IEventRepository eventRepository, IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserResponse>> Handle(ListUsersInEventQuery request,
        CancellationToken cancellationToken)
    {
        var eventId = Guid.Parse(request.EventId);
        var users = await _userRepository.GetUsersByEventIdAsync(eventId, cancellationToken);

        var userResponses = _mapper.Map<IEnumerable<UserResponse>>(users);
        
        return userResponses;
    }
}