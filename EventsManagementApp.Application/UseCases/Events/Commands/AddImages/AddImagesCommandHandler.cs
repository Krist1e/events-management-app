using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using MediatR;

namespace EventsManagementApp.Application.UseCases.Events.Commands.AddImages;

public class AddImagesCommandHandler : IRequestHandler<AddImagesCommand, AddImagesResponse>
{
    private readonly IImageService _imageService;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddImagesCommandHandler(IImageService imageService, IImageRepository imageRepository, IUnitOfWork unitOfWork)
    {
        _imageService = imageService;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddImagesResponse> Handle(AddImagesCommand request, CancellationToken cancellationToken)
    {
        var tasks = request.Images.ImageFiles.Select(f => _imageService.UploadImageAsync(f, cancellationToken));
        var images = await Task.WhenAll(tasks);
        var eventId = Guid.Parse(request.EventId);

        await _imageRepository.AddImagesToEventAsync(eventId, images, cancellationToken);
        await _unitOfWork.CommitChangesAsync(cancellationToken);

        var imageUrls = images.Select(i => i.ImageUrl).ToArray();
        return new AddImagesResponse(imageUrls);
    }
}