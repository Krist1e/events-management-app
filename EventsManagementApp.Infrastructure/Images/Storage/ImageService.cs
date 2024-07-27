using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EventsManagementApp.Infrastructure.Images.Storage;

public class ImageService : IImageService
{
    public Task<Image> UploadImageAsync(IFormFile imageFile)
    {
        Console.WriteLine("Uploading image...");
        return Task.FromResult(new Image());
    }
}