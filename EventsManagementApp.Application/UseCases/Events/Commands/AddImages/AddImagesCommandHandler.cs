using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.AddImages;

public class AddImagesCommandHandler : IRequestHandler<AddImagesCommand, AddImagesResponse>
{
    private readonly IImageService _imageService;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddImagesCommandHandler> _logger;

    public AddImagesCommandHandler(IImageService imageService, IImageRepository imageRepository, IUnitOfWork unitOfWork,
        ILogger<AddImagesCommandHandler> logger)
    {
        _imageService = imageService;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AddImagesResponse> Handle(AddImagesCommand request, CancellationToken cancellationToken)
    {
        var tasks = request.Images.ImageFiles.Select(f => _imageService.UploadImageAsync(f, cancellationToken));
        var images = await Task.WhenAll(tasks);
        var eventId = Guid.Parse(request.EventId);

        var result = await _imageRepository.AddImagesToEventAsync(eventId, images, cancellationToken);

        if (!result)
        {
            throw new Exception("Failed to add images to event");
        }

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Images added to event with id {EventId}", eventId);

        var imageResponses = images.Select(i => new ImageResponse(i.Id.ToString(), i.ImageUrl));
        return new AddImagesResponse(imageResponses);
    }
}