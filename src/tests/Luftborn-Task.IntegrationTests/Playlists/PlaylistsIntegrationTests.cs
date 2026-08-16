using FluentAssertions;
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Playlist.Application.Playlist.CreatePlaylist;
using Module.Playlist.Application.Playlist.UpdatePlaylist;
using Module.Playlist.Application.PlaylistSongs.AddSong;
using Module.Playlist.Application.PlaylistSongs.GetUserPlaylist;
using Module.Songs.Application.Songs.CreateSong;
using Module.Users.Application.Users.RegisterUser;
using Test.Common.Domain;

namespace Luftborn_Task.IntegrationTests.Playlists;

public class PlaylistsIntegrationTests : BaseIntegrationTest
{
    public PlaylistsIntegrationTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCreatePlaylist()
    {
        // Arrange
        Result<Guid> userResult = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid userId = userResult.Value;
        string playlistName = Faker.Name.FirstName();

        // Act
        Result<Guid> response = await Sender.Send(new CreatePlaylistCommand(userId, playlistName));

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenUpdatePlaylist()
    {
        // Arrange
        Result<Guid> userResult = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid userId = userResult.Value;
        Result<Guid> playlistResult = await Sender.Send(new CreatePlaylistCommand(userId, Faker.Name.FirstName()));
        Guid playlistId = playlistResult.Value;

        string updatedName = Faker.Name.FirstName();

        // Act
        Result response = await Sender.Send(new UpdatePlaylistCommand(playlistId, updatedName, userId));

        // Assert
        response.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenAddSongToPlaylist()
    {
        // Arrange
        Result<Guid> publisherResult = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid publisherId = publisherResult.Value;

        Result<Guid> songResult = await Sender.Send(new CreateSongCommand(Faker.Name.FirstName(), Faker.Random.Number(1, 1000), publisherId));
        Guid songId = songResult.Value;

        Result<Guid> ownerResult = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid ownerId = ownerResult.Value;

        Result<Guid> playlistResult = await Sender.Send(new CreatePlaylistCommand(ownerId, Faker.Name.FirstName()));
        Guid playlistId = playlistResult.Value;

        // Act
        Result addResult = await Sender.Send(new AddSongCommand(playlistId, songId, ownerId));

        // Assert
        addResult.IsSuccess.Should().BeTrue();

        Result<IEnumerable<UserPlaylistResponse>> playlistSongsResult = await Sender.Send(new GetUserPlaylistQuery(ownerId, playlistId));
        playlistSongsResult.IsSuccess.Should().BeTrue();
        playlistSongsResult.Value.Should().ContainSingle(s => s.SongId == songId);
    }
}
