using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Users.Application.Users.GetUser;

namespace Luftborn_Task.IntegrationTests.Users;

public class UserEndpointIntegrationTests : BaseIntegrationTest
{
    public UserEndpointIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRegisterUserEndpointCalled()
    {
        // Arrange
        var request = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("users/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        Guid userId = await response.Content.ReadFromJsonAsync<Guid>();
        userId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_ReturnOk_WhenGetUserProfileEndpointCalled()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage registerResponse = await HttpClient.PostAsJsonAsync("users/register", registerRequest);
        Guid userId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"users/profile?id={userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        UserResponse? userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();
        userResponse.Should().NotBeNull();
        userResponse!.FirstName.Should().Be(registerRequest.FirstName);
        userResponse.LastName.Should().Be(registerRequest.LastName);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenUpdateUserEndpointCalled()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage registerResponse = await HttpClient.PostAsJsonAsync("users/register", registerRequest);
        Guid userId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        var updateRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName()
        };

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"/users/{userId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        HttpResponseMessage profileResponse = await HttpClient.GetAsync($"users/profile?id={userId}");
        UserResponse? userResponse = await profileResponse.Content.ReadFromJsonAsync<UserResponse>();
        userResponse!.FirstName.Should().Be(updateRequest.FirstName);
        userResponse.LastName.Should().Be(updateRequest.LastName);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenDeleteUserEndpointCalled()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage registerResponse = await HttpClient.PostAsJsonAsync("users/register", registerRequest);
        Guid userId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"/api/users/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
