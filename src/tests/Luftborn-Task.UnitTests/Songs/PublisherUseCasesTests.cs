using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Application.Publisher.CreatePublisher;
using Module.Songs.Application.Publisher.DeletePublisher;
using Module.Songs.Application.Publisher.UpdatePublisher;
using Module.Songs.Domain.Publisher;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class PublisherUseCasesTests
{
    [Fact]
    public async Task CreatePublisher_ShouldReturnSuccess()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var handler = new CreatePublisherCommandHandler(publisherRepository.Object, unitOfWork.Object);
        var command = new CreatePublisherCommand(publisherId, "John", "Doe", "john@example.com");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        publisherRepository.Verify(x => x.Insert(It.Is<Publisher>(p => p.Id == publisherId && p.FirstName == "John" && p.LastName == "Doe" && p.Email == "john@example.com")), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisher_ShouldReturnSuccess_WhenPublisherExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = Publisher.Create(publisherId, "John", "Doe", "john@example.com");
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(publisher);

        var handler = new UpatePublisherCommandHandler(publisherRepository.Object, unitOfWork.Object);
        var command = new UpdatePublisherCommand(publisherId, "Johnny", "Smith");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Johnny", publisher.FirstName);
        Assert.Equal("Smith", publisher.LastName);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisher_ShouldReturnFailure_WhenPublisherDoesNotExist()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Publisher?)null);

        var handler = new UpatePublisherCommandHandler(publisherRepository.Object, unitOfWork.Object);
        var command = new UpdatePublisherCommand(publisherId, "Johnny", "Smith");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Publisher.NotFound", result.Error.Code);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeletePublisher_ShouldReturnSuccess_WhenPublisherExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = Publisher.Create(publisherId, "John", "Doe", "john@example.com");
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(publisher);

        var handler = new DeletePublisherCommandHandler(publisherRepository.Object, unitOfWork.Object);
        var command = new DeletePublisherCommand(publisherId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        publisherRepository.Verify(x => x.Delete(publisher), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletePublisher_ShouldReturnFailure_WhenPublisherDoesNotExist()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Publisher?)null);

        var handler = new DeletePublisherCommandHandler(publisherRepository.Object, unitOfWork.Object);
        var command = new DeletePublisherCommand(publisherId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Publisher.NotFound", result.Error.Code);
        publisherRepository.Verify(x => x.Delete(It.IsAny<Publisher>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
