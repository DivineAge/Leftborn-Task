
using FluentAssertions;
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Songs.Application.Songs.CreateSong;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Application.Songs.UpdateSong;
using Module.Songs.Domain.Songs;
using Module.Users.Application.Users.RegisterUser;
using Test.Common.Domain;

namespace Luftborn_Task.IntegrationTests.Songs;

public class SongsIntegrationsTests : BaseIntegrationTest
{
    public SongsIntegrationsTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
    [Fact]
    public async Task Should_ReturnError_WhenSongDoesNotExist()
    {
        // Arrange
        var songId = Guid.NewGuid();

        // Act
        Result<SongResponse> response = await Sender.Send(new GetSongQuery(songId));

        // Assert

        response.Error.Should().Be(SongErrors.NotFound(songId));
    }

    [Fact]
    public async Task Should_ReturnSong_WhenSongExists()
    {
        // Arrange
        Result<Guid> resultUser = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Result<Guid> result = await Sender.Send(new CreateSongCommand(Faker.Name.FirstName(), Faker.Random.Number(1, 1000), resultUser.Value));
        Guid songId = result.Value;

        // Act 
        Result<SongResponse> response = await Sender.Send(new GetSongQuery(songId));

        // Assert
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenUpdateSong()
    {
        // Arrange
        Result<Guid> resultUser = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid publisherId = resultUser.Value;
        Result<Guid> resultSong = await Sender.Send(new CreateSongCommand(Faker.Name.FirstName(), Faker.Random.Number(1, 1000), publisherId));
        Guid songId = resultSong.Value;

        string updatedSongName = Faker.Name.FirstName();
        int updatedDuration = Faker.Random.Number(1, 1000);

        // Act
        Result updateResult = await Sender.Send(new UpdateSongCommand(songId, publisherId, updatedSongName, updatedDuration));

        // Assert
        updateResult.IsSuccess.Should().BeTrue();

        Result<SongResponse> response = await Sender.Send(new GetSongQuery(songId));
        response.IsSuccess.Should().BeTrue();
        response.Value.Name.Should().Be(updatedSongName);
        response.Value.TimeInSeconds.Should().Be(updatedDuration);
    }

    [Fact]
    public async Task Should_ReturnError_WhenUpdateSong_WhenSongDoesNotExist()
    {
        // Arrange
        Result<Guid> resultUser = await Sender.Send(new RegisterUserCommand(Faker.Name.FirstName(), Faker.Name.LastName(), Faker.Internet.Email()));
        Guid publisherId = resultUser.Value;
        Guid songId = Guid.NewGuid();

        // Act
        Result updateResult = await Sender.Send(new UpdateSongCommand(songId, publisherId, Faker.Name.FirstName(), Faker.Random.Number(1, 1000)));

        // Assert
        updateResult.Error.Should().Be(SongErrors.NotFound(songId));
    }
}
