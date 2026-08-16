
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Users.Domain.Users;
using Module.Users.Application.Users.GetUser;
using Test.Common.Domain;
namespace Luftborn_Task.IntegrationTests.Users;

using FluentAssertions;
using Module.Users.Application.Users.DeleteUser;
using Module.Users.Application.Users.RegisterUser;
using Module.Users.Application.Users.UpdateUser;

public class UserTestIntegrationTests : BaseIntegrationTest
{
    public UserTestIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
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

    [Fact]
    public async Task Should_ReturnSuccess_WhenUpdateUser()
    {
        // Arrange
        Result<Guid> result = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid userId = result.Value;
        string updatedFirstName = Faker.Name.FirstName();
        string updatedLastName = Faker.Name.LastName();

        // Act
        Result updateResult = await Sender.Send(new UpdateUserCommand(userId, updatedFirstName, updatedLastName));

        // Assert
        updateResult.IsSuccess.Should().BeTrue();

        Result<UserResponse> response = await Sender.Send(new GetUserQuery(userId));
        response.IsSuccess.Should().BeTrue();
        response.Value.FirstName.Should().Be(updatedFirstName);
        response.Value.LastName.Should().Be(updatedLastName);
    }

    [Fact]
    public async Task Should_ReturnError_WhenUpdateUser_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        Result updateResult = await Sender.Send(new UpdateUserCommand(userId, Faker.Name.FirstName(), Faker.Name.LastName()));

        // Assert
        updateResult.Error.Should().Be(UserError.NotFound(userId));
    }
}
