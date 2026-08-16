using Module.Playlist.PublicApi;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Application.Songs.CreateSong;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class CreateSongTests
{
    [Fact]
    public async Task CreateSong_ShouldReturnSuccess_WhenPublisherExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = Publisher.Create(publisherId, "John", "Doe", "john@example.com");

        var songRepository = new Mock<ISongRepository>();
        var playlistApi = new Mock<IPlaylistApi>();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(publisher);

        var handler = new CreateSongCommandHandler(
            songRepository.Object,
            playlistApi.Object,
            publisherRepository.Object,
            unitOfWork.Object);

        var command = new CreateSongCommand("Test Song", 200, publisherId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);

        songRepository.Verify(x => x.Insert(It.Is<Song>(s => s.Name == "Test Song" && s.TimeInSeconds == 200 && s.PublisherId == publisherId)), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        playlistApi.Verify(x => x.CreateSongAsync(result.Value, publisherId, 200, "Test Song", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateSong_ShouldReturnFailure_WhenPublisherDoesNotExist()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var songRepository = new Mock<ISongRepository>();
        var playlistApi = new Mock<IPlaylistApi>();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Publisher?)null);

        var handler = new CreateSongCommandHandler(
            songRepository.Object,
            playlistApi.Object,
            publisherRepository.Object,
            unitOfWork.Object);

        var command = new CreateSongCommand("Test Song", 200, publisherId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Publisher.NotFound", result.Error.Code);

        songRepository.Verify(x => x.Insert(It.IsAny<Song>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        playlistApi.Verify(x => x.CreateSongAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
