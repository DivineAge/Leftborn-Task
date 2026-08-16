using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Application.Songs.CreateSong;
using Module.Playlist.Application.Songs.DeleteSong;
using Module.Playlist.Application.Songs.UpdateSong;
using Module.Playlist.Application.User.CreateUser;
using Module.Playlist.Application.User.DeleteUser;
using Module.Playlist.Application.User.UpdateUser;
using Module.Playlist.Domain.Songs;
using Module.Playlist.Domain.Users;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Playlists;

public class PlaylistUserSongSyncTests
{
    [Fact]
    public async Task CreateUserSync_ShouldReturnSuccess_WhenEmailIsUnique()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        userRepository.Setup(x => x.GetByEmailAsync("sync@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new CreateUserCommandHandler(userRepository.Object, unitOfWork.Object);
        var command = new CreateUserCommand(userId, "John", "Doe", "sync@example.com");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        userRepository.Verify(x => x.Insert(It.Is<User>(u => u.Id == userId && u.Email == "sync@example.com")), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserSync_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = User.Create(Guid.NewGuid(), "John", "Doe", "sync@example.com");
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        userRepository.Setup(x => x.GetByEmailAsync("sync@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var handler = new CreateUserCommandHandler(userRepository.Object, unitOfWork.Object);
        var command = new CreateUserCommand(Guid.NewGuid(), "Johnny", "Smith", "sync@example.com");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("User.EmailAlreadyExists", result.Error.Code);
        userRepository.Verify(x => x.Insert(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserSync_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(userId, "John", "Doe", "john@example.com");
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        userRepository.Setup(x => x.GetAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new UpdateUserCommandHandler(userRepository.Object, unitOfWork.Object);
        var command = new UpdateUserCommand(userId, "Jane", "Smith");

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Jane", user.FirstName);
        Assert.Equal("Smith", user.LastName);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUserSync_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create(userId, "John", "Doe", "john@example.com");
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        userRepository.Setup(x => x.GetAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var handler = new DeleteUserCommandHandler(userRepository.Object, unitOfWork.Object);
        var command = new DeleteUserCommand(userId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        userRepository.Verify(x => x.Delete(user), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateSongSync_ShouldReturnSuccess_WhenPublisherUserExists()
    {
        // Arrange
        var songId = Guid.NewGuid();
        var publisherId = Guid.NewGuid();
        var publisherUser = User.Create(publisherId, "Publisher", "User", "pub@example.com");
        var songRepository = new Mock<ISongRepository>();
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        userRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(publisherUser);

        var handler = new CreateSongCommandHandler(songRepository.Object, userRepository.Object, unitOfWork.Object);
        var command = new CreateSongCommand(songId, "Sync Song", 180, publisherId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        songRepository.Verify(x => x.Insert(It.Is<Song>(s => s.Id == songId && s.Name == "Sync Song" && s.TimeInSeconds == 180 && s.PublisherId == publisherId)), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateSongSync_ShouldReturnSuccess_WhenSongAndPublisherUserExist()
    {
        // Arrange
        var songId = Guid.NewGuid();
        var publisherId = Guid.NewGuid();
        var song = Song.Create(songId, publisherId, 180, "Original Title");
        var publisherUser = User.Create(publisherId, "Publisher", "User", "pub@example.com");
        var songRepository = new Mock<ISongRepository>();
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(songId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);
        userRepository.Setup(x => x.GetAsync(publisherId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(publisherUser);

        var handler = new UpdateSongCommandHandler(songRepository.Object, userRepository.Object, unitOfWork.Object);
        var command = new UpdateSongCommand(songId, publisherId, "Updated Title", 220);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated Title", song.Name);
        Assert.Equal(220, song.TimeInSeconds);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteSongSync_ShouldReturnSuccess_WhenSongExists()
    {
        // Arrange
        var songId = Guid.NewGuid();
        var song = Song.Create(songId, Guid.NewGuid(), 180, "To Delete");
        var songRepository = new Mock<ISongRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();

        songRepository.Setup(x => x.GetAsync(songId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(song);

        var handler = new DeleteSongCommandHandler(unitOfWork.Object, songRepository.Object);
        var command = new DeleteSongCommand(songId);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        songRepository.Verify(x => x.Delete(song), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
