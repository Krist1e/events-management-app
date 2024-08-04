using AutoMapper;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using FluentValidation;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Users.Queries.ListUsersInEvent;

public class ListUsersInEventQueryHandler : IRequestHandler<ListUsersInEventQuery, IEnumerable<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<ListUsersInEventQuery> _validator;

    public ListUsersInEventQueryHandler(IUserRepository userRepository, IMapper mapper,
        IValidator<ListUsersInEventQuery> validator)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IEnumerable<UserResponse>> Handle(ListUsersInEventQuery request,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var eventId = Guid.Parse(request.EventId);
        var users = await _userRepository.GetUsersByEventIdAsync(eventId, cancellationToken);

        var userResponses = _mapper.Map<IEnumerable<UserResponse>>(users);

        return userResponses;
    }
}