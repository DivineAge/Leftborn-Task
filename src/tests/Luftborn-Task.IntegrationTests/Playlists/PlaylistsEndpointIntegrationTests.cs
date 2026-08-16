using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Playlist.Application.PlaylistSongs.GetUserPlaylist;

namespace Luftborn_Task.IntegrationTests.Playlists;

public class PlaylistsEndpointIntegrationTests : BaseIntegrationTest
{
    public PlaylistsEndpointIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnOk_WhenCreatePlaylistEndpointCalled()
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

        var createPlaylistRequest = new
        {
            UserId = userId,
            Name = Faker.Name.FirstName()
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/playlist", createPlaylistRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        Guid playlistId = await response.Content.ReadFromJsonAsync<Guid>();
        playlistId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_ReturnOk_WhenUpdatePlaylistEndpointCalled()
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

        var createPlaylistRequest = new
        {
            UserId = userId,
            Name = Faker.Name.FirstName()
        };
        HttpResponseMessage createResponse = await HttpClient.PostAsJsonAsync("/api/playlist", createPlaylistRequest);
        Guid playlistId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var updateRequest = new
        {
            Name = Faker.Name.FirstName(),
            OwnerId = userId
        };

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"/api/playlists/id/{playlistId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenAddSongToPlaylistEndpointCalled()
    {
        // Arrange
        var publisherRegister = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage pubResponse = await HttpClient.PostAsJsonAsync("users/register", publisherRegister);
        Guid publisherId = await pubResponse.Content.ReadFromJsonAsync<Guid>();

        var createSongRequest = new
        {
            Name = Faker.Name.FirstName(),
            TimeInSeconds = Faker.Random.Number(1, 1000),
            PublisherId = publisherId
        };
        HttpResponseMessage songResponse = await HttpClient.PostAsJsonAsync("songs", createSongRequest);
        Guid songId = await songResponse.Content.ReadFromJsonAsync<Guid>();

        var ownerRegister = new
        {
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
            Email = Faker.Internet.Email()
        };
        HttpResponseMessage ownerResponse = await HttpClient.PostAsJsonAsync("users/register", ownerRegister);
        Guid ownerId = await ownerResponse.Content.ReadFromJsonAsync<Guid>();

        var createPlaylistRequest = new
        {
            UserId = ownerId,
            Name = Faker.Name.FirstName()
        };
        HttpResponseMessage playlistResponse = await HttpClient.PostAsJsonAsync("/api/playlist", createPlaylistRequest);
        Guid playlistId = await playlistResponse.Content.ReadFromJsonAsync<Guid>();

        var addSongRequest = new
        {
            PlaylistId = playlistId,
            SongId = songId,
            OwnerId = ownerId
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/playlists/addsong/", addSongRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        HttpResponseMessage getPlaylistResponse = await HttpClient.GetAsync($"/api/users/{ownerId}/playlists/{playlistId}");
        getPlaylistResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        IEnumerable<UserPlaylistResponse>? playlistSongs = await getPlaylistResponse.Content.ReadFromJsonAsync<IEnumerable<UserPlaylistResponse>>();
        playlistSongs.Should().ContainSingle(s => s.SongId == songId);
    }
}
