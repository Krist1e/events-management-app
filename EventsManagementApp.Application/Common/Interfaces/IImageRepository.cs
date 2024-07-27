using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IImageRepository
{
    Task<Image?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken);
    Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(Image image, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Image image, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Image image, CancellationToken cancellationToken);
    
    Task<IEnumerable<Image>> GetImagesByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task<bool> AddImageToEventAsync(Guid eventId, Image image, CancellationToken cancellationToken);
    Task<bool> AddImagesToEventAsync(Guid eventId, IEnumerable<Image> images, CancellationToken cancellationToken);
    Task<bool> RemoveImageFromEventAsync(Guid eventId, Guid imageId, CancellationToken cancellationToken);
}