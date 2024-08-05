using AutoMapper;
using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Commands.AddImages;
using EventsManagementApp.Application.UseCases.Events.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Events.Commands;

public class AddImagesCommandHandlerTests
{
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddImagesCommandHandler _handler;

    public AddImagesCommandHandlerTests()
    {
        _imageServiceMock = new Mock<IImageService>();
        _imageRepositoryMock = new Mock<IImageRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        Mock<ILogger<AddImagesCommandHandler>> loggerMock = new();
        _mapperMock = new Mock<IMapper>();
        Mock<IValidator<AddImagesCommand>> validatorMock = new();

        _handler = new AddImagesCommandHandler(
            _imageServiceMock.Object,
            _imageRepositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object,
            _mapperMock.Object,
            validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenAddImagesFails()
    {
        // Arrange
        var command = new AddImagesCommand(new FormFileCollection(), Guid.NewGuid().ToString());

        _imageRepositoryMock.Setup(r =>
                r.AddImagesToEventAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Image>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<AddImagesFailedException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(5)]
    [InlineData(0)]
    public async Task Handle_ShouldReturnImageResponses_WhenAddImagesSucceeds(int imageCount)
    {
        // Arrange
        var imageFiles = new FormFileCollection();
        imageFiles.AddRange(Enumerable.Range(0, imageCount)
            .Select(_ => new FormFile(Stream.Null, 0, 0, "test.jpg", "test.jpg")));

        var command = new AddImagesCommand(imageFiles, Guid.NewGuid().ToString());

        var images = Enumerable.Range(0, imageCount).Select(_ => new Image());

        _imageRepositoryMock.Setup(r =>
                r.AddImagesToEventAsync(It.IsAny<Guid>(), It.IsAny<IEnumerable<Image>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _imageServiceMock.Setup(s => s.UploadImageAsync(It.IsAny<IFormFile>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Image());

        _mapperMock.Setup(m => m.Map<IEnumerable<ImageResponse>>(It.IsAny<IEnumerable<Image>>()))
            .Returns(images.Select(i => new ImageResponse(Guid.NewGuid().ToString(), "test.jpg")));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(imageCount, result.Count());
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}