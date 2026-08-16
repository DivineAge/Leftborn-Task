using Module.Playlist.PublicApi;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Application.Songs.UpdateSong;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class UpdateSongTests
{
    [Fact]
    public async Task UpdateSong_ShouldReturnSuccess_WhenSongAndPublisherExist()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var song = Song.Create(publisherId, 200, "Old Name");
        var publisher = Publisher.Create(publisherId, "John", "Doe", "john@example.com");

        var songRepository = new Mock<ISongRepository>();
        var playlistApi = new Mock<IPlaylistApi>();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);
        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(publisher);

        var handler = new UpdateSongCommandHandler(
            songRepository.Object,
            playlistApi.Object,
            publisherRepository.Object,
            unitOfWork.Object);

        var command = new UpdateSongCommand(song.Id, publisherId, "New Name", 250);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(250, song.TimeInSeconds);
        Assert.Equal("New Name", song.Name);

        playlistApi.Verify(x => x.UpdateSongAsync(song.Id, publisherId, 250, "New Name", It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSong_ShouldReturnFailure_WhenSongDoesNotExist()
    {
        // Arrange
        var songId = Guid.NewGuid();
        var publisherId = Guid.NewGuid();

        var songRepository = new Mock<ISongRepository>();
        var playlistApi = new Mock<IPlaylistApi>();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(songId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Song?)null);

        var handler = new UpdateSongCommandHandler(
            songRepository.Object,
            playlistApi.Object,
            publisherRepository.Object,
            unitOfWork.Object);

        var command = new UpdateSongCommand(songId, publisherId, "New Name", 250);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Song.NotFound", result.Error.Code);

        playlistApi.Verify(x => x.UpdateSongAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateSong_ShouldReturnFailure_WhenPublisherDoesNotExist()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var song = Song.Create(publisherId, 200, "Old Name");

        var songRepository = new Mock<ISongRepository>();
        var playlistApi = new Mock<IPlaylistApi>();
        var publisherRepository = new Mock<IPublisherRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);
        publisherRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Publisher?)null);

        var handler = new UpdateSongCommandHandler(
            songRepository.Object,
            playlistApi.Object,
            publisherRepository.Object,
            unitOfWork.Object);

        var command = new UpdateSongCommand(song.Id, publisherId, "New Name", 250);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Publisher.NotFound", result.Error.Code);

        playlistApi.Verify(x => x.UpdateSongAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
