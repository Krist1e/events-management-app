using System.Collections;
using Asp.Versioning;
using EventsManagementApp.Application.UseCases.Events.Commands.AddImages;
using EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;
using EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using EventsManagementApp.Application.UseCases.Events.Queries.GetEventById;
using EventsManagementApp.Application.UseCases.Events.Queries.ListEvents;
using EventsManagementApp.Application.UseCases.Users.Commands.RegisterInEvent;
using EventsManagementApp.Application.UseCases.Users.Commands.UnregisterFromEvent;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagementApp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/events")]
public class EventsController : ControllerBase
{
    private readonly ISender _sender;

    public EventsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventResponse>>> GetEvents(CancellationToken cancellationToken)
    {
        var events = await _sender.Send(new ListEventsQuery(), cancellationToken);
        return Ok(events);
    }

    [HttpGet("{eventId}")]
    public async Task<ActionResult<CreateEventResponse>> GetEventById(string eventId,
        CancellationToken cancellationToken)
    {
        var eventResponse = await _sender.Send(new GetEventByIdQuery(eventId), cancellationToken);
        return Ok(eventResponse);
    }

    [HttpPost]
    public async Task<ActionResult<CreateEventResponse>> CreateEvent([FromBody] CreateEventRequest createEventRequest,
        CancellationToken cancellationToken)
    {
        var eventResponse = await _sender.Send(new CreateEventCommand(createEventRequest), cancellationToken);
        return Ok(eventResponse);
    }

    [HttpPost("{eventId}/images")]
    public async Task<ActionResult<IEnumerable<ImageResponse>>> AddImagesToEventAsync([FromForm] IFormFileCollection imageFiles,
        string eventId, CancellationToken cancellationToken)
    {
        var imageResponse = await _sender.Send(new AddImagesCommand(imageFiles, eventId), cancellationToken);
        return Ok(imageResponse);
    }

    [HttpDelete("images")]
    public async Task<IActionResult> RemoveImagesAsync([FromBody] IEnumerable<string> imageIds, CancellationToken cancellationToken)
    {
        await _sender.Send(new RemoveImagesCommand(imageIds), cancellationToken);
        return Ok();
    }

    [HttpPost("{eventId}/users")]
    public async Task<IActionResult> RegisterUserToEvent(string eventId, CancellationToken cancellationToken)
    {
        var userId = User.Identity?.GetUserId();
        
        if (userId is null)
        {
            return BadRequest("User not found");
        }
        
        await _sender.Send(new RegisterInEventCommand(userId, eventId), cancellationToken);
        return Ok();
    }
    
    [HttpDelete("{eventId}/users")]
    public async Task<IActionResult> UnregisterUserFromEvent(string eventId, CancellationToken cancellationToken)
    {
        var userId = User.Identity?.GetUserId();
        
        if (userId is null)
        {
            return BadRequest("User not found");
        }
        
        await _sender.Send(new UnregisterFromEventCommand(userId, eventId), cancellationToken);
        return Ok();
    }
}