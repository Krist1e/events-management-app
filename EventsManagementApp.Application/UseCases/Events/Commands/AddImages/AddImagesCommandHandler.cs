using AutoMapper;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.AddImages;

public class AddImagesCommandHandler : IRequestHandler<AddImagesCommand, IEnumerable<ImageResponse>>
{
    private readonly IImageService _imageService;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddImagesCommandHandler> _logger;
    private readonly IMapper _mapper;

    public AddImagesCommandHandler(IImageService imageService, IImageRepository imageRepository, IUnitOfWork unitOfWork,
        ILogger<AddImagesCommandHandler> logger, IMapper mapper)
    {
        _imageService = imageService;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ImageResponse>> Handle(AddImagesCommand request, CancellationToken cancellationToken)
    {
        var tasks = request.ImageFiles.Select(f => _imageService.UploadImageAsync(f, cancellationToken));
        var images = await Task.WhenAll(tasks);
        var eventId = Guid.Parse(request.EventId);

        var result = await _imageRepository.AddImagesToEventAsync(eventId, images, cancellationToken);

        if (!result)
        {
            throw new AddImagesFailedException("Failed to add images to event");
        }

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        _logger.LogInformation("Images added to event with id {EventId}", eventId);

        var imageResponses = _mapper.Map<IEnumerable<ImageResponse>>(images);
        
        return imageResponses;
    }
}