using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IImageRepository
{
    Task<Image?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken);
    Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken);
    Task CreateAsync(Image image, CancellationToken cancellationToken);
    Task UpdateAsync(Image image, CancellationToken cancellationToken);
    Task DeleteAsync(Image image, CancellationToken cancellationToken);
    
    Task<IEnumerable<Image>> GetImagesByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task AddImageToEventAsync(Guid eventId, Guid imageId, CancellationToken cancellationToken);
    Task RemoveImageFromEventAsync(Guid eventId, Guid imageId, CancellationToken cancellationToken);
}