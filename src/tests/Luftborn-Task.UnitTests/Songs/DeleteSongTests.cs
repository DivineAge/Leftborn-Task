using Module.Playlist.PublicApi;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Application.Songs.DeleteSong;
using Module.Songs.Domain.Songs;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class DeleteSongTests
{
    [Fact]
    public async Task DeleteSong_ShouldReturnSuccess_WhenSongExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var song = Song.Create(publisherId, 200, "Song To Delete");

        var songRepository = new Mock<ISongRepository>();
        var playlistApi = new Mock<IPlaylistApi>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);

        var handler = new DeleteSongCommandHandler(
            playlistApi.Object,
            songRepository.Object,
            unitOfWork.Object);

        var command = new DeleteSongCommand(song.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);

        playlistApi.Verify(x => x.DeleteSongAsync(song.Id, It.IsAny<CancellationToken>()), Times.Once);
        songRepository.Verify(x => x.Delete(song), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSong_ShouldReturnFailure_WhenSongDoesNotExist()
    {
        // Arrange
        var songId = Guid.NewGuid();

        var songRepository = new Mock<ISongRepository>();
        var playlistApi = new Mock<IPlaylistApi>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(songId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Song?)null);

        var handler = new DeleteSongCommandHandler(
            playlistApi.Object,
            songRepository.Object,
            unitOfWork.Object);

        var command = new DeleteSongCommand(songId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Song.NotFound", result.Error.Code);

        playlistApi.Verify(x => x.DeleteSongAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        songRepository.Verify(x => x.Delete(It.IsAny<Song>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
