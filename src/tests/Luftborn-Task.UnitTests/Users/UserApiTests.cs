using MediatR;
using Module.Users.Application.Users.GetUser;
using Module.Users.Domain.Users;
using Module.Users.Infrastructure.PublicApi;
using Moq;
using Xunit;

namespace Luftborn_Task.UnitTests.Users;

public class UserApiTests
{
    [Fact]
    public async Task UserApi_GetUserAsync_ShouldReturnUserResponse_WhenSenderReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var appResponse = new Module.Users.Application.Users.GetUser.UserResponse(userId, "John", "Doe", "john@example.com");

        var senderMock = new Mock<ISender>();
        senderMock.Setup(s => s.Send(It.Is<GetUserQuery>(q => q.Id == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(appResponse);

        var userApi = new UserApi(senderMock.Object);

        // Act
        var result = await userApi.GetUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
    }

    [Fact]
    public async Task UserApi_GetUserAsync_ShouldReturnNull_WhenSenderReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var senderMock = new Mock<ISender>();
        senderMock.Setup(s => s.Send(It.Is<GetUserQuery>(q => q.Id == userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Test.Common.Domain.Result.Failure<Module.Users.Application.Users.GetUser.UserResponse>(UserError.NotFound(userId)));

        var userApi = new UserApi(senderMock.Object);

        // Act
        var result = await userApi.GetUserAsync(userId);

        // Assert
        Assert.Null(result);
    }
}
