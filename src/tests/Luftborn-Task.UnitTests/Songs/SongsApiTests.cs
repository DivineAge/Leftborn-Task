using MediatR;
using Module.Songs.Application.Publisher.CreatePublisher;
using Module.Songs.Application.Publisher.DeletePublisher;
using Module.Songs.Application.Publisher.UpdatePublisher;
using Module.Songs.Infrastructure.PublicApi;
using Moq;
using Test.Common.Domain;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class SongsApiTests
{
    [Fact]
    public async Task CreatePublisherAsync_ShouldSendCreatePublisherCommand()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<CreatePublisherCommand>(c =>
            c.PublisherId == publisherId &&
            c.FirstName == "John" &&
            c.LastName == "Doe" &&
            c.Email == "john@example.com"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var songsApi = new SongsApi(senderMock.Object);

        // Act
        await songsApi.CreatePublisherAsync(publisherId, "John", "Doe", "john@example.com");

        // Assert
        senderMock.Verify(s => s.Send(It.Is<CreatePublisherCommand>(c =>
            c.PublisherId == publisherId &&
            c.FirstName == "John" &&
            c.LastName == "Doe" &&
            c.Email == "john@example.com"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePublisherAsync_ShouldSendUpdatePublisherCommand()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<UpdatePublisherCommand>(c =>
            c.PublisherId == publisherId &&
            c.FirstName == "Jane" &&
            c.LastName == "Smith"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var songsApi = new SongsApi(senderMock.Object);

        // Act
        await songsApi.UpdatePublisherAsync(publisherId, "Jane", "Smith");

        // Assert
        senderMock.Verify(s => s.Send(It.Is<UpdatePublisherCommand>(c =>
            c.PublisherId == publisherId &&
            c.FirstName == "Jane" &&
            c.LastName == "Smith"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletePublisherAsync_ShouldSendDeletePublisherCommand()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<DeletePublisherCommand>(c => c.Id == publisherId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var songsApi = new SongsApi(senderMock.Object);

        // Act
        await songsApi.DeletePublisherAsync(publisherId);

        // Assert
        senderMock.Verify(s => s.Send(It.Is<DeletePublisherCommand>(c => c.Id == publisherId), It.IsAny<CancellationToken>()), Times.Once);
    }
}
