using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventsManagementApp.Application.UseCases.Events.Commands.AddImages;

public record AddImagesCommand(IFormFileCollection ImageFiles, string EventId) : IRequest<IEnumerable<ImageResponse>>;