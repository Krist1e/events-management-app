using EventManagementApp.Domain.Entities;
using EventsManagementApp.Application.Common.Exceptions;
using EventsManagementApp.Application.Common.Interfaces;
using EventsManagementApp.Application.UseCases.Events.Commands.RemoveImages;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EventsManagementApp.Test.UseCases.Events.Commands;

public class RemoveImagesCommandTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RemoveImagesCommandHandler _handler;

    public RemoveImagesCommandTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        Mock<ILogger<RemoveImagesCommandHandler>> loggerMock = new();
        Mock<IImageService> imageServiceMock = new();
        Mock<IValidator<RemoveImagesCommand>> validatorMock = new();

        _handler = new RemoveImagesCommandHandler(
            _imageRepositoryMock.Object,
            _unitOfWorkMock.Object,
            imageServiceMock.Object,
            loggerMock.Object,
            validatorMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldRemoveImages_WhenImagesExist()
    {
        // Arrange
        var imageIds = new List<string> { Guid.NewGuid().ToString() };
        var command = new RemoveImagesCommand(imageIds);

        _imageRepositoryMock.Setup(r =>
                r.GetImagesByIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Image> { new() });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _imageRepositoryMock.Verify(r => r.DeleteImages(It.IsAny<IEnumerable<Image>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowImagesNotFoundException_WhenImagesDoNotExist()
    {
        // Arrange
        var imageIds = new List<string> { Guid.NewGuid().ToString() };
        var command = new RemoveImagesCommand(imageIds);

        _imageRepositoryMock.Setup(r =>
                r.GetImagesByIdsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Image>());

        // Act & Assert
        await Assert.ThrowsAsync<ImagesNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        _unitOfWorkMock.Verify(u => u.CommitChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _imageRepositoryMock.Verify(r => r.DeleteImages(It.IsAny<IEnumerable<Image>>()), Times.Never);
    }
}