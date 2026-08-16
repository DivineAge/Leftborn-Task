using Module.Playlist.PublicApi;
using Module.Songs.PublicApi;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Application.Users.UpdateUser;
using Module.Users.Domain.Users;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Users;

public class UpdateUserTests
{
    [Fact]
    public async Task UpdateUser_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var existingUser = User.Create("Alice", "Brown", "alice@example.com");
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var songsApi = new Mock<ISongsApi>();
        var playlistApi = new Mock<IPlaylistApi>();

        userRepository.Setup(x => x.GetAsync(existingUser.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var handler = new UpdateUserCommandHandler(userRepository.Object, songsApi.Object, playlistApi.Object, unitOfWork.Object);

        // Act
        var result = await handler.Handle(new UpdateUserCommand(existingUser.Id, "Alice", "Smith"), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Alice", existingUser.FirstName);
        Assert.Equal("Smith", existingUser.LastName);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        songsApi.Verify(x => x.UpdatePublisherAsync(existingUser.Id, "Alice", "Smith", It.IsAny<CancellationToken>()), Times.Once);
        playlistApi.Verify(x => x.UpdateUserAsync(existingUser.Id, "Alice", "Smith", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateUser_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var songsApi = new Mock<ISongsApi>();
        var playlistApi = new Mock<IPlaylistApi>();

        userRepository.Setup(x => x.GetAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new UpdateUserCommandHandler(userRepository.Object, songsApi.Object, playlistApi.Object, unitOfWork.Object);

        // Act
        var result = await handler.Handle(new UpdateUserCommand(userId, "Updated", "User"), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Users.NotFound", result.Error.Code);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        songsApi.Verify(x => x.UpdatePublisherAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        playlistApi.Verify(x => x.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
