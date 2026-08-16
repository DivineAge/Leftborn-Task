
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Playlist.Domain.Users;
using Module.Users.Application.Users.GetUser;
using Test.Common.Domain;
namespace Luftborn_Task.IntegrationTests.Users;

using FluentAssertions;
using Module.Playlist.Application.User.DeleteUser;
using Module.Users.Application.Users.RegisterUser;

public class UserTestIntegrationTests : BaseIntegrationTest, IClassFixture<IntegrationTestWebAppFactory>
{
    protected UserTestIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    [Fact]
    public async Task Shoud_ReturnError_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Result<UserResponse> response = await Sender.Send(new GetUserQuery(userId));

        // Assert

        response.Error.Should().Be(UserError.NotFound(userId));


    }
    [Fact]
    public async Task Should_ReturnUser_WhenUserExists()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid userId = result.Value;

        // Act 
        Result<UserResponse> response = await Sender.Send(new GetUserQuery(userId));

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();


    }
    [Fact]
    public async Task Should_ReturnSuccuess_WhenDeleteUser()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid userId = result.Value;

        // Act 
        Result response = await Sender.Send(new DeleteUserCommand(userId));

        // Assert
        response.IsSuccess.Should().BeTrue();

    }
}
