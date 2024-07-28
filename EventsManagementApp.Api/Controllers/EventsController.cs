using Asp.Versioning;
using EventsManagementApp.Application.UseCases.Events.Commands.AddImages;
using EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;
using EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EventsManagementApp.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/events")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly ISender _sender;

    public EventsController(ILogger<EventsController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet]
    public IActionResult GetEvents()
    {
        return Ok("Events");
    }

    [HttpPost]
    public async Task<ActionResult<EventResponse>> CreateEvent([FromBody] EventRequest eventRequest,
        CancellationToken cancellationToken)
    {
        var eventResponse = await _sender.Send(new CreateEventCommand(eventRequest), cancellationToken);
        return Ok(eventResponse);
    }

    [HttpPost("{eventId}/images")]
    public async Task<ActionResult<AddImagesResponse>> AddImagesToEventAsync([FromForm] AddImagesRequest request,
        string eventId, CancellationToken cancellationToken)
    {
        var imageResponse = await _sender.Send(new AddImagesCommand(request, eventId), cancellationToken);
        return imageResponse;
    }

    [HttpDelete("{eventId}/images")]
    public async Task<ActionResult<bool>> RemoveImagesFromEventAsync([FromBody] RemoveImagesRequest request,
        string eventId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new RemoveImagesCommand(request, eventId), cancellationToken);
        return result;
    }
}