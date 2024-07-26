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
    Task<bool> AddImageToEventAsync(EventImage eventImage, CancellationToken cancellationToken);
    Task<bool> RemoveImageFromEventAsync(EventImage eventImage, CancellationToken cancellationToken);
}