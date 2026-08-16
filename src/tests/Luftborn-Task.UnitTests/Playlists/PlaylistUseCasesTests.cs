using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Application.Playlist.CreatePlaylist;
using Module.Playlist.Application.Playlist.DeletePlaylist;
using Module.Playlist.Application.Playlist.UpdatePlaylist;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.Users;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Playlists;

public class PlaylistUseCasesTests
{
    [Fact]
    public async Task CreatePlaylist_ShouldReturnSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        var handler = new CreatePlaylistCommandHandler(playlistRepository.Object, unitOfWork.Object);
        var command = new CreatePlaylistCommand(userId, "Rock Legends");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        playlistRepository.Verify(x => x.Insert(It.Is<Playlist>(p => p.Name == "Rock Legends" && p.OwnerId == userId)), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePlaylist_ShouldReturnSuccess_WhenPlaylistAndUserExist()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var playlist = Playlist.Create(ownerId, "Old Name");
        var user = User.Create(ownerId, "John", "Doe", "john@example.com");

        var playlistRepository = new Mock<IPlaylistRepository>();
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        playlistRepository.Setup(x => x.GetAsync(playlist.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);
        userRepository.Setup(x => x.GetAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new UpdatePlaylistCommandHandler(playlistRepository.Object, userRepository.Object, unitOfWork.Object);
        var command = new UpdatePlaylistCommand(playlist.Id, "New Name", ownerId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("New Name", playlist.Name);
        Assert.Equal(ownerId, playlist.OwnerId);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePlaylist_ShouldReturnFailure_WhenPlaylistDoesNotExist()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var ownerId = Guid.NewGuid();

        var playlistRepository = new Mock<IPlaylistRepository>();
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        playlistRepository.Setup(x => x.GetAsync(playlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Playlist?)null);

        var handler = new UpdatePlaylistCommandHandler(playlistRepository.Object, userRepository.Object, unitOfWork.Object);
        var command = new UpdatePlaylistCommand(playlistId, "New Name", ownerId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Playlist.NotFound", result.Error.Code);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdatePlaylist_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var playlist = Playlist.Create(ownerId, "Old Name");

        var playlistRepository = new Mock<IPlaylistRepository>();
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        playlistRepository.Setup(x => x.GetAsync(playlist.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);
        userRepository.Setup(x => x.GetAsync(ownerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new UpdatePlaylistCommandHandler(playlistRepository.Object, userRepository.Object, unitOfWork.Object);
        var command = new UpdatePlaylistCommand(playlist.Id, "New Name", ownerId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("User.NotFound", result.Error.Code);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeletePlaylist_ShouldReturnSuccess_WhenPlaylistExists()
    {
        // Arrange
        var playlist = Playlist.Create(Guid.NewGuid(), "Playlist To Delete");

        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        playlistRepository.Setup(x => x.GetAsync(playlist.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(playlist);

        var handler = new DeletePlaylistCommandHandler(playlistRepository.Object, unitOfWork.Object);
        var command = new DeletePlaylistCommand(playlist.Id);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        playlistRepository.Verify(x => x.Delete(playlist), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeletePlaylist_ShouldReturnFailure_WhenPlaylistDoesNotExist()
    {
        // Arrange
        var playlistId = Guid.NewGuid();

        var playlistRepository = new Mock<IPlaylistRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        playlistRepository.Setup(x => x.GetAsync(playlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Playlist?)null);

        var handler = new DeletePlaylistCommandHandler(playlistRepository.Object, unitOfWork.Object);
        var command = new DeletePlaylistCommand(playlistId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Playlist.NotFound", result.Error.Code);
        playlistRepository.Verify(x => x.Delete(It.IsAny<Playlist>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
