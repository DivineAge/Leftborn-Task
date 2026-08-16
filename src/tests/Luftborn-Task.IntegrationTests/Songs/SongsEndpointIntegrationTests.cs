using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Songs.Application.Songs.GetSong;

namespace Luftborn_Task.IntegrationTests.Songs;

public class SongsEndpointIntegrationTests : BaseIntegrationTest
{
    public SongsEndpointIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnOk_WhenCreateSongEndpointCalled()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage registerResponse = await HttpClient.PostAsJsonAsync("users/register", registerRequest);
        Guid publisherId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        var createSongRequest = new
        {
            Name = Faker.Name.FirstName(),
            TimeInSeconds = Faker.Random.Number(1, 1000),
            PublisherId = publisherId
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("songs", createSongRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        Guid songId = await response.Content.ReadFromJsonAsync<Guid>();
        songId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_ReturnOk_WhenGetSongByIdEndpointCalled()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage registerResponse = await HttpClient.PostAsJsonAsync("users/register", registerRequest);
        Guid publisherId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        var createSongRequest = new
        {
            Name = Faker.Name.FirstName(),
            TimeInSeconds = Faker.Random.Number(1, 1000),
            PublisherId = publisherId
        };
        HttpResponseMessage createSongResponse = await HttpClient.PostAsJsonAsync("songs", createSongRequest);
        Guid songId = await createSongResponse.Content.ReadFromJsonAsync<Guid>();

        // Act
        HttpResponseMessage response = await HttpClient.GetAsync($"songs/id/{songId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        SongResponse? songResponse = await response.Content.ReadFromJsonAsync<SongResponse>();
        songResponse.Should().NotBeNull();
        songResponse!.Name.Should().Be(createSongRequest.Name);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenUpdateSongEndpointCalled()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage registerResponse = await HttpClient.PostAsJsonAsync("users/register", registerRequest);
        Guid publisherId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        var createSongRequest = new
        {
            Name = Faker.Name.FirstName(),
            TimeInSeconds = Faker.Random.Number(1, 1000),
            PublisherId = publisherId
        };
        HttpResponseMessage createSongResponse = await HttpClient.PostAsJsonAsync("songs", createSongRequest);
        Guid songId = await createSongResponse.Content.ReadFromJsonAsync<Guid>();

        var updateSongRequest = new
        {
            PublisherId = publisherId,
            Name = Faker.Name.FirstName(),
            TimeInSeconds = Faker.Random.Number(1, 1000)
        };

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"/api/songs/id/{songId}", updateSongRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        HttpResponseMessage getResponse = await HttpClient.GetAsync($"songs/id/{songId}");
        SongResponse? songResponse = await getResponse.Content.ReadFromJsonAsync<SongResponse>();
        songResponse!.Name.Should().Be(updateSongRequest.Name);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenDeleteSongEndpointCalled()
    {
        // Arrange
        var registerRequest = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage registerResponse = await HttpClient.PostAsJsonAsync("users/register", registerRequest);
        Guid publisherId = await registerResponse.Content.ReadFromJsonAsync<Guid>();

        var createSongRequest = new
        {
            Name = Faker.Name.FirstName(),
            TimeInSeconds = Faker.Random.Number(1, 1000),
            PublisherId = publisherId
        };
        HttpResponseMessage createSongResponse = await HttpClient.PostAsJsonAsync("songs", createSongRequest);
        Guid songId = await createSongResponse.Content.ReadFromJsonAsync<Guid>();

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"/api/songs/{songId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
