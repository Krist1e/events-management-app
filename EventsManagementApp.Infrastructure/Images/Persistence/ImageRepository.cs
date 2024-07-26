using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventsManagementApp.Infrastructure.Images.Persistence;

public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ImageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Image?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken)
    {
        var image = await _dbContext.Images.FirstOrDefaultAsync(i => i.Id == imageId, cancellationToken);
        return image;
    }

    public async Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken)
    {
        var images = await _dbContext.Images.ToListAsync(cancellationToken);
        return images;
    }

    public async Task CreateAsync(Image image, CancellationToken cancellationToken)
    {
        await _dbContext.Images.AddAsync(image, cancellationToken);
    }

    public async Task<bool> UpdateAsync(Image image, CancellationToken cancellationToken)
    {
        var existingImage = await GetByIdAsync(image.Id, cancellationToken);

        if (existingImage is null)
            return false;

        _dbContext.Images.Update(image);
        return true;
    }

    public async Task<bool> DeleteAsync(Image image, CancellationToken cancellationToken)
    {
        var existingImage = await GetByIdAsync(image.Id, cancellationToken);

        if (existingImage is null)
            return false;

        _dbContext.Images.Remove(image);
        return true;
    }

    public async Task<IEnumerable<Image>> GetImagesByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var images = await _dbContext.Images.Where(i => i.EventImages.Any(ei => ei.EventId == eventId))
            .ToListAsync(cancellationToken);
        return images;
    }

    public async Task<bool> AddImageToEventAsync(EventImage eventImage, CancellationToken cancellationToken)
    {
        var existingEventImage = await _dbContext.EventImages.FirstOrDefaultAsync(ei =>
            ei.EventId == eventImage.EventId && ei.ImageId == eventImage.ImageId, cancellationToken);

        if (existingEventImage is null)
            return false;

        await _dbContext.EventImages.AddAsync(eventImage, cancellationToken);
        return true;
    }

    public async Task<bool> RemoveImageFromEventAsync(EventImage eventImage, CancellationToken cancellationToken)
    {
        var existingEventImage = await _dbContext.EventImages.FirstOrDefaultAsync(ei =>
            ei.EventId == eventImage.EventId && ei.ImageId == eventImage.ImageId, cancellationToken);

        if (existingEventImage is null)
            return false;

        _dbContext.EventImages.Remove(eventImage);
        return true;
    }
}