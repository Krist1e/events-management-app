using EventManagementApp.Domain.Entities;
using EventManagementApp.Domain.Enums;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.CreateEvent;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageService _imageService;
    private readonly IUserRepository _userRepository;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IImageService imageService, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _userRepository = userRepository;
    }

    public async Task<EventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var images = new List<Image>();

        foreach (var imageFile in request.Event.ImageFiles)
        {
            var image = await _imageService.UploadImageAsync(imageFile);
            images.Add(image);
        }
        
        var newEvent = new Event
        {
            Name = request.Event.Name,
            Description = request.Event.Description,
            StartDate = request.Event.StartDate,
            EndDate = request.Event.EndDate,
            Location = request.Event.Location,
            Category = Enum.Parse<CategoryEnum>(request.Event.Category),
            Capacity = request.Event.Capacity,
            Images = images
        };
        
        var userEvent = new UserEvent
        {
            UserId = Guid.Parse(request.Event.UserId),
            Event = newEvent,
            IsOrganizer = true,
            RegistrationDate = DateTime.UtcNow
        };
        
        await _userRepository.AddUserToEventAsync(userEvent, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        return new EventResponse
        (
            newEvent.Id,
            newEvent.Name,
            newEvent.Description,
            newEvent.StartDate,
            newEvent.EndDate,
            newEvent.Location,
            newEvent.Category.ToString(),
            newEvent.Capacity,
            newEvent.Images.Select(i => i.ImageUrl).ToList(),
            userEvent.UserId.ToString()
        );
    }
}