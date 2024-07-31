using Asp.Versioning;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using EventsManagementApp.Application.UseCases.Events.Queries.ListEventsByUserId;
using EventsManagementApp.Application.UseCases.Users.Contracts;
using EventsManagementApp.Application.UseCases.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagementApp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;
    
    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserResponse>> GetUserById(string userId, CancellationToken cancellationToken)
    {
        var userResponse = await _sender.Send(new GetUserByIdQuery(userId), cancellationToken);

        return Ok(userResponse);
    }

    [HttpGet("{userId}/events")]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEventsByUserId(string userId,
        CancellationToken cancellationToken)
    {
        var events = await _sender.Send(new ListEventsByUserIdQuery(userId), cancellationToken);

        return Ok(events);
    }
}