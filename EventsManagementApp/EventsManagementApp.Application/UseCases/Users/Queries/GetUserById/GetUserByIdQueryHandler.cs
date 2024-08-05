using AutoMapper;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IValidator<GetUserByIdQuery> _validator;

    public GetUserByIdQueryHandler(IUserRepository userRepository, ILogger<GetUserByIdQueryHandler> logger,
        IMapper mapper, IValidator<GetUserByIdQuery> validator)
    {
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<UserResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var userId = Guid.Parse(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("User with id {UserId} was not found", userId);
            throw new UserNotFoundException($"User with id {userId} was not found");
        }

        var userResponse = _mapper.Map<UserResponse>(user);
        
        return userResponse;
    }
}