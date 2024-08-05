using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace EventsManagementApp.Infrastructure.Images.Storage;

public class ImageService : IImageService
{
    private readonly StorageClient _storageClient;
    private readonly string? _bucketName;

    public ImageService(IConfiguration configuration)
    {
        var section = configuration.GetSection("GoogleCloud");
        var googleCredential = GoogleCredential.FromFile(section.GetValue<string>("GoogleCredentialFile"));
        _storageClient = StorageClient.Create(googleCredential);
        _bucketName = section.GetValue<string>("GoogleCloudStorageBucket");
    }
    
    public async Task<Image> UploadImageAsync(IFormFile imageFile, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        
        await imageFile.CopyToAsync(memoryStream, cancellationToken);
        var imageStorageName = DateTime.Now.ToString("yyyyMMddHHmmss") + imageFile.FileName;
        var dataObject = await _storageClient.UploadObjectAsync(_bucketName, imageStorageName, null, memoryStream, cancellationToken: cancellationToken);
            
        var image = new Image
        {
            ImageUrl = dataObject.MediaLink,
            ImageStorageName = imageStorageName
        };
            
        return image;
    }
    
    public async Task<bool> RemoveImageAsync(string imageStorageName, CancellationToken cancellationToken)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, imageStorageName, cancellationToken: cancellationToken);
        return true;
    }
}