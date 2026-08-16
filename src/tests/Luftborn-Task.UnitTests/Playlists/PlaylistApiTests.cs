using MediatR;
using Module.Playlist.Application.Songs.CreateSong;
using Module.Playlist.Application.Songs.DeleteSong;
using Module.Playlist.Application.Songs.UpdateSong;
using Module.Playlist.Application.User.CreateUser;
using Module.Playlist.Application.User.DeleteUser;
using Module.Playlist.Application.User.UpdateUser;
using Module.Playlist.Infrastructure.PublicApi;
using Moq;
using Test.Common.Domain;
using Xunit;

namespace Luftborn_Task.UnitTests.Playlists;

public class PlaylistApiTests
{
    [Fact]
    public async Task CreateUserAsync_ShouldSendCreateUserCommand()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<CreateUserCommand>(c =>
            c.UserId == userId &&
            c.FirstName == "John" &&
            c.LastName == "Doe" &&
            c.Email == "john@example.com"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var playlistApi = new PlaylistApi(senderMock.Object);

        // Act
        await playlistApi.CreateUserAsync(userId, "John", "Doe", "john@example.com");

        // Assert
        senderMock.Verify(s => s.Send(It.Is<CreateUserCommand>(c =>
            c.UserId == userId &&
            c.FirstName == "John" &&
            c.LastName == "Doe" &&
            c.Email == "john@example.com"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldSendUpdateUserCommand()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<UpdateUserCommand>(c =>
            c.UserId == userId &&
            c.FirstName == "Jane" &&
            c.LastName == "Smith"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var playlistApi = new PlaylistApi(senderMock.Object);

        // Act
        await playlistApi.UpdateUserAsync(userId, "Jane", "Smith");

        // Assert
        senderMock.Verify(s => s.Send(It.Is<UpdateUserCommand>(c =>
            c.UserId == userId &&
            c.FirstName == "Jane" &&
            c.LastName == "Smith"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldSendDeleteUserCommand()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<DeleteUserCommand>(c => c.Id == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var playlistApi = new PlaylistApi(senderMock.Object);

        // Act
        await playlistApi.DeleteUserAsync(userId);

        // Assert
        senderMock.Verify(s => s.Send(It.Is<DeleteUserCommand>(c => c.Id == userId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateSongAsync_ShouldSendCreateSongCommand()
    {
        // Arrange
        var songId = Guid.NewGuid();
        var publisherId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<CreateSongCommand>(c =>
            c.SongId == songId &&
            c.PublisherId == publisherId &&
            c.TimeInSeconds == 180 &&
            c.Name == "New Track"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var playlistApi = new PlaylistApi(senderMock.Object);

        // Act
        await playlistApi.CreateSongAsync(songId, publisherId, 180, "New Track");

        // Assert
        senderMock.Verify(s => s.Send(It.Is<CreateSongCommand>(c =>
            c.SongId == songId &&
            c.PublisherId == publisherId &&
            c.TimeInSeconds == 180 &&
            c.Name == "New Track"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSongAsync_ShouldSendUpdateSongCommand()
    {
        // Arrange
        var songId = Guid.NewGuid();
        var publisherId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<UpdateSongCommand>(c =>
            c.SongId == songId &&
            c.PublisherId == publisherId &&
            c.TimeInSeconds == 210 &&
            c.Name == "Updated Track"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var playlistApi = new PlaylistApi(senderMock.Object);

        // Act
        await playlistApi.UpdateSongAsync(songId, publisherId, 210, "Updated Track");

        // Assert
        senderMock.Verify(s => s.Send(It.Is<UpdateSongCommand>(c =>
            c.SongId == songId &&
            c.PublisherId == publisherId &&
            c.TimeInSeconds == 210 &&
            c.Name == "Updated Track"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSongAsync_ShouldSendDeleteSongCommand()
    {
        // Arrange
        var songId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();

        senderMock.Setup(s => s.Send(It.Is<DeleteSongCommand>(c => c.Id == songId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var playlistApi = new PlaylistApi(senderMock.Object);

        // Act
        await playlistApi.DeleteSongAsync(songId);

        // Assert
        senderMock.Verify(s => s.Send(It.Is<DeleteSongCommand>(c => c.Id == songId), It.IsAny<CancellationToken>()), Times.Once);
    }
}
