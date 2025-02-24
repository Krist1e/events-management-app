﻿using EventManagementApp.Domain.Entities;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IImageRepository
{
    Task<Image?> GetByIdAsync(Guid imageId, CancellationToken cancellationToken);
    Task<IEnumerable<Image>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Image image, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Image image, CancellationToken cancellationToken);
    
    Task<List<Image>> GetImagesByIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken);
    void DeleteImages(IEnumerable<Image> images);
    Task<bool> AddImagesToEventAsync(Guid eventId, IEnumerable<Image> images, CancellationToken cancellationToken);
}