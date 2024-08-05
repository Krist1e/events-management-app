using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;

public class RemoveImagesCommandHandler : IRequestHandler<RemoveImagesCommand>
{
    private readonly IImageRepository _imageRepository;
    private readonly IImageService _imageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RemoveImagesCommandHandler> _logger;
    private readonly IValidator<RemoveImagesCommand> _validator;

    public RemoveImagesCommandHandler(IImageRepository imageRepository, IUnitOfWork unitOfWork,
        IImageService imageService, ILogger<RemoveImagesCommandHandler> logger,
        IValidator<RemoveImagesCommand> validator)
    {
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
        _imageService = imageService;
        _logger = logger;
        _validator = validator;
    }

    public async Task Handle(RemoveImagesCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        
        var images = await _imageRepository.GetImagesByIdsAsync(request.ImageIds, cancellationToken);

        if (images.Count == 0)
        {
            _logger.LogWarning("Images with ids {ImageIds} not found", string.Join(", ", request.ImageIds));
            throw new ImagesNotFoundException("Images not found");
        }

        var tasks = images.Select(i => _imageService.RemoveImageAsync(i.ImageStorageName, cancellationToken));
        await Task.WhenAll(tasks);

        _imageRepository.DeleteImages(images);

        await _unitOfWork.CommitChangesAsync(cancellationToken);

        _logger.LogInformation("Images with ids {ImageIds} removed", string.Join(", ", request.ImageIds));
    }
}