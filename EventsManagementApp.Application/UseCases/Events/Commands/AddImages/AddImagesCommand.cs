using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.AddImages;

public record AddImagesCommand(AddImagesRequest Images, string EventId) : IRequest<AddImagesResponse>;