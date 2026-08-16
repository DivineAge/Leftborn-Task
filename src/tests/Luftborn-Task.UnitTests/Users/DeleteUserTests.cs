using Module.Playlist.PublicApi;
using Module.Songs.PublicApi;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Application.Users.DeleteUser;
using Module.Users.Domain.Users;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Users;

public class DeleteUserTests
{
    [Fact]
    public async Task DeleteUser_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var existingUser = User.Create("Delete", "User", "delete@example.com");
        var userRepository = new Mock<IUserRepository>();
        var songsApi = new Mock<ISongsApi>();
        var playlistApi = new Mock<IPlaylistApi>();
        var unitOfWork = new Mock<IUnitOfWork>();

        userRepository.Setup(x => x.GetAsync(existingUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var handler = new DeleteUserCommandHandler(userRepository.Object, songsApi.Object, playlistApi.Object, unitOfWork.Object);

        // Act
        var result = await handler.Handle(new DeleteUserCommand(existingUser.Id), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        songsApi.Verify(x => x.DeletePublisherAsync(existingUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        playlistApi.Verify(x => x.DeleteUserAsync(existingUser.Id, It.IsAny<CancellationToken>()), Times.Once);
        userRepository.Verify(x => x.Delete(existingUser), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRepository = new Mock<IUserRepository>();
        var songsApi = new Mock<ISongsApi>();
        var playlistApi = new Mock<IPlaylistApi>();
        var unitOfWork = new Mock<IUnitOfWork>();

        userRepository.Setup(x => x.GetAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new DeleteUserCommandHandler(userRepository.Object, songsApi.Object, playlistApi.Object, unitOfWork.Object);

        // Act
        var result = await handler.Handle(new DeleteUserCommand(userId), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Users.NotFound", result.Error.Code);
        userRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        songsApi.Verify(x => x.DeletePublisherAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        playlistApi.Verify(x => x.DeleteUserAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
