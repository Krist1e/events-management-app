using EventManagementApp.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EventsManagementApp.Application.Common.Interfaces;

public interface IImageService
{
    Task<Image> UploadImageAsync(IFormFile imageFile);
}