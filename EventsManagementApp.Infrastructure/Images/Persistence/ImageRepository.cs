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
        var images = await _dbContext.Images.Where(i => i.EventId == eventId).ToListAsync(cancellationToken);
        return images;
    }

    public async Task<bool> AddImageToEventAsync(Guid eventId, Image image, CancellationToken cancellationToken)
    {
        var existingEvent = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
        var existingImage = await _dbContext.Images.FirstOrDefaultAsync(i => i.Id == image.Id, cancellationToken);

        if (existingEvent is null || existingImage is null)
            return false;

        existingEvent.Images.Add(existingImage);
        return true;
    }

    public async Task<bool> AddImagesToEventAsync(Guid eventId, IEnumerable<Image> images,
        CancellationToken cancellationToken)
    {
        var existingEvent = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (existingEvent is null)
            return false;

        existingEvent.Images.AddRange(images);
        return true;
    }

    public bool DeleteImages(IEnumerable<Image> images)
    {
        _dbContext.Images.RemoveRange(images);
        return true;
    }

    public async Task<IEnumerable<Image>> GetImagesByImageUrlsAsync(Guid eventId, IEnumerable<string> imageUrls,
        CancellationToken cancellationToken)
    {
        var images = await _dbContext.Images.Where(i => i.EventId == eventId && imageUrls.Contains(i.ImageUrl))
            .ToListAsync(cancellationToken);
        return images;
    }
}