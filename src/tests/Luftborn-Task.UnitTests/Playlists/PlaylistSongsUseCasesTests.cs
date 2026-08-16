using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Application.PlaylistSongs.AddSong;
using Module.Playlist.Application.PlaylistSongs.RemoveSong;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.PlaylistSongs;
using Module.Playlist.Domain.Songs;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Playlists;

public class PlaylistSongsUseCasesTests
{
    [Fact]
    public async Task AddSong_ShouldReturnSuccess_WhenSongAndPlaylistExistAndUserIsOwner()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var playlist = Playlist.Create(ownerId, "Party Playlist");
        var song = Song.Create(Guid.NewGuid(), ownerId, 210, "Song 1");

        var playlistSongsRepository = new Mock<IPlaylistSongsRepository>();
        var songRepository = new Mock<ISongRepository>();
        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);
        playlistRepository.Setup(x => x.GetAsync(playlist.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);
        playlistSongsRepository.Setup(x => x.GetAsync(playlist.Id, song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((PlaylistSong?)null);

        var handler = new AddSongCommandHandler(
            playlistSongsRepository.Object,
            songRepository.Object,
            playlistRepository.Object,
            unitOfWork.Object);

        var command = new AddSongCommand(playlist.Id, song.Id, ownerId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        playlistSongsRepository.Verify(x => x.Insert(It.Is<PlaylistSong>(ps => ps.PlaylistId == playlist.Id && ps.SongId == song.Id)), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddSong_ShouldReturnFailure_WhenSongDoesNotExist()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var playlistId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        var playlistSongsRepository = new Mock<IPlaylistSongsRepository>();
        var songRepository = new Mock<ISongRepository>();
        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(songId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Song?)null);

        var handler = new AddSongCommandHandler(
            playlistSongsRepository.Object,
            songRepository.Object,
            playlistRepository.Object,
            unitOfWork.Object);

        var command = new AddSongCommand(playlistId, songId, ownerId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Song.NotFound", result.Error.Code);
        playlistSongsRepository.Verify(x => x.Insert(It.IsAny<PlaylistSong>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task AddSong_ShouldReturnFailure_WhenPlaylistDoesNotExist()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var playlistId = Guid.NewGuid();
        var song = Song.Create(Guid.NewGuid(), ownerId, 210, "Song 1");

        var playlistSongsRepository = new Mock<IPlaylistSongsRepository>();
        var songRepository = new Mock<ISongRepository>();
        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);
        playlistRepository.Setup(x => x.GetAsync(playlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Playlist?)null);

        var handler = new AddSongCommandHandler(
            playlistSongsRepository.Object,
            songRepository.Object,
            playlistRepository.Object,
            unitOfWork.Object);

        var command = new AddSongCommand(playlistId, song.Id, ownerId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Playlist.NotFound", result.Error.Code);
        playlistSongsRepository.Verify(x => x.Insert(It.IsAny<PlaylistSong>()), Times.Never);
    }

    [Fact]
    public async Task AddSong_ShouldReturnFailure_WhenUserIsNotPlaylistOwner()
    {
        // Arrange
        var actualOwnerId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();
        var playlist = Playlist.Create(actualOwnerId, "Party Playlist");
        var song = Song.Create(Guid.NewGuid(), actualOwnerId, 210, "Song 1");

        var playlistSongsRepository = new Mock<IPlaylistSongsRepository>();
        var songRepository = new Mock<ISongRepository>();
        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);
        playlistRepository.Setup(x => x.GetAsync(playlist.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);

        var handler = new AddSongCommandHandler(
            playlistSongsRepository.Object,
            songRepository.Object,
            playlistRepository.Object,
            unitOfWork.Object);

        var command = new AddSongCommand(playlist.Id, song.Id, differentUserId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Playlist.NotOwner", result.Error.Code);
        playlistSongsRepository.Verify(x => x.Insert(It.IsAny<PlaylistSong>()), Times.Never);
    }

    [Fact]
    public async Task RemoveSong_ShouldReturnSuccess_WhenSongExistsInPlaylistAndUserIsOwner()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var playlist = Playlist.Create(ownerId, "Party Playlist");
        var song = Song.Create(Guid.NewGuid(), ownerId, 210, "Song 1");
        var playlistSong = PlaylistSong.Create(playlist.Id, song.Id);

        var playlistSongsRepository = new Mock<IPlaylistSongsRepository>();
        var songRepository = new Mock<ISongRepository>();
        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);
        playlistRepository.Setup(x => x.GetAsync(playlist.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);
        playlistSongsRepository.Setup(x => x.GetAsync(playlist.Id, song.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlistSong);

        var handler = new RemoveSongCommandHandler(
            playlistSongsRepository.Object,
            songRepository.Object,
            playlistRepository.Object,
            unitOfWork.Object);

        var command = new RemoveSongCommand(playlist.Id, song.Id, ownerId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        playlistSongsRepository.Verify(x => x.Delete(playlistSong), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
