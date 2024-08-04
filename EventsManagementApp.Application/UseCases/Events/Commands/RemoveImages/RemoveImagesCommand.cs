using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;

public record RemoveImagesCommand(IEnumerable<string> ImageIds) : IRequest;