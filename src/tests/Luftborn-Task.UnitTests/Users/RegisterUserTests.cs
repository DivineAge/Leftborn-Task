using Module.Playlist.PublicApi;
using Module.Songs.PublicApi;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Application.Users.RegisterUser;
using Module.Users.Domain.Users;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Users;

public class RegisterUserTests
{
    [Fact]
    public async Task RegisterUser_ShouldReturnSuccess_WhenEmailDoesNotExist()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var songsApi = new Mock<ISongsApi>();
        var playlistApi = new Mock<IPlaylistApi>();

        userRepository.Setup(x => x.GetByEmailAsync("john@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var handler = new RegisterUserCommandHandler(unitOfWork.Object, songsApi.Object, playlistApi.Object, userRepository.Object);

        // Act
        var result = await handler.Handle(new RegisterUserCommand("John", "Doe", "john@example.com"), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);

        userRepository.Verify(x => x.Insert(It.Is<User>(u => u.FirstName == "John" && u.LastName == "Doe" && u.Email == "john@example.com")), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        songsApi.Verify(x => x.CreatePublisherAsync(result.Value, "John", "Doe", "john@example.com", It.IsAny<CancellationToken>()), Times.Once);
        playlistApi.Verify(x => x.CreateUserAsync(result.Value, "John", "Doe", "john@example.com", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = User.Create("Jane", "Doe", "jane@example.com");
        var userRepository = new Mock<IUserRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var songsApi = new Mock<ISongsApi>();
        var playlistApi = new Mock<IPlaylistApi>();

        userRepository.Setup(x => x.GetByEmailAsync("jane@example.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        var handler = new RegisterUserCommandHandler(unitOfWork.Object, songsApi.Object, playlistApi.Object, userRepository.Object);

        // Act
        var result = await handler.Handle(new RegisterUserCommand("Jane", "Doe", "jane@example.com"), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Users.EmailAlreadyExists", result.Error.Code);
        userRepository.Verify(x => x.Insert(It.IsAny<User>()), Times.Never);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        songsApi.Verify(x => x.CreatePublisherAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        playlistApi.Verify(x => x.CreateUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
