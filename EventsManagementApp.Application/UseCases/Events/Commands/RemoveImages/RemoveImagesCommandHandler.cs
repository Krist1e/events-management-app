using EventsManagementApp.Application.Common.Interfaces;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;

public class RemoveImagesCommandHandler : IRequestHandler<RemoveImagesCommand, bool>
{
    private readonly IImageRepository _imageRepository;
    private readonly IImageService _imageService;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveImagesCommandHandler(IImageRepository imageRepository, IUnitOfWork unitOfWork, IImageService imageService)
    {
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
    }

    public async Task<bool> Handle(RemoveImagesCommand request, CancellationToken cancellationToken)
    {
        var eventId = Guid.Parse(request.EventId);
        var images =
            (await _imageRepository.GetImagesByImageUrlsAsync(eventId, request.Images.ImageUrls, cancellationToken))
            .ToList();
        
        if (images.Count == 0)
            return false;

        var tasks = images.Select(i => _imageService.RemoveImageAsync(i.ImageStorageName, cancellationToken));
        await Task.WhenAll(tasks);

        var result = _imageRepository.DeleteImages(images);
        
        if (!result)
            return false;
        
        await _unitOfWork.CommitChangesAsync(cancellationToken);
        return true;
    }
}