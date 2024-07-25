using Asp.Versioning;
using EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventsManagementApp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/events")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly Mediator _mediator;

    public EventsController(ILogger<EventsController> logger, Mediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetEvents()
    {
        return Ok("Events");
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<EventResponse>> CreateEvent([FromBody] EventRequest request, CancellationToken cancellationToken)
    {
        var eventResponse = await _mediator.Send(new CreateEventCommand(request), cancellationToken);
        
        if (eventResponse == null)
        {
            return BadRequest();
        }
        
        return Ok(eventResponse);
    }
    
}