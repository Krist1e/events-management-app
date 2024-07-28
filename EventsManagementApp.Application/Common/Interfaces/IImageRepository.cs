using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IImageRepository
{
    Task<Image?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken);
    Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Image image, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Image image, CancellationToken cancellationToken);
    
    Task<IEnumerable<Image>> GetImagesByImageUrlsAsync(Guid eventId, IEnumerable<string> imageUrls, CancellationToken cancellationToken);
    bool DeleteImages(IEnumerable<Image> images);
    Task<bool> AddImagesToEventAsync(Guid eventId, IEnumerable<Image> images, CancellationToken cancellationToken);
}