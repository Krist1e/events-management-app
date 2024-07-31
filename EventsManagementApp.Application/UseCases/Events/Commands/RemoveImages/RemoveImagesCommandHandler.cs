using EventsManagementApp.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;

public class RemoveImagesCommandHandler : IRequestHandler<RemoveImagesCommand>
{
    private readonly IImageRepository _imageRepository;
    private readonly IImageService _imageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveImagesCommandHandler> _logger;

    public RemoveImagesCommandHandler(IImageRepository imageRepository, IUnitOfWork unitOfWork,
        IImageService imageService, ILogger<RemoveImagesCommandHandler> logger)
    {
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _logger = logger;
    }

    public async Task Handle(RemoveImagesCommand request, CancellationToken cancellationToken)
    {
        var images = await _imageRepository.GetImagesByIdsAsync(request.Images.ImageIds, cancellationToken);

        if (images.Count == 0)
        {
            _logger.LogWarning("Images with ids {ImageIds} not found", string.Join(", ", request.Images.ImageIds));
            throw new ImagesNotFoundException("Images not found");
        }

        var tasks = images.Select(i => _imageService.RemoveImageAsync(i.ImageStorageName, cancellationToken));
        await Task.WhenAll(tasks);

        var result = _imageRepository.DeleteImages(images);

        if (!result)
        {
            _logger.LogWarning("Failed to remove images with ids {ImageIds}", string.Join(", ", request.Images.ImageIds));
            throw new RemoveImagesFailedException("Failed to remove images");
        }

        await _unitOfWork.CommitChangesAsync(cancellationToken);
        
        _logger.LogInformation("Images with ids {ImageIds} removed", string.Join(", ", request.Images.ImageIds));
    }
}