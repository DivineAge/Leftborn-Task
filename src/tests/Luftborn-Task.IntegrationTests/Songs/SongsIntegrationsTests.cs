
using FluentAssertions;
using Luftborn_Task.IntegrationTests.Abstractions;
using Module.Songs.Application.Songs.CreateSong;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Domain.Songs;
using Module.Users.Application.Users.RegisterUser;
using Test.Common.Domain;

namespace Luftborn_Task.IntegrationTests.Songs;

public class SongsIntegrationsTests : BaseIntegrationTest, IClassFixture<IntegrationTestWebAppFactory>
{
    protected SongsIntegrationsTests(IntegrationTestWebAppFactory factory) : base(factory)
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

        response.Error.Should().Be(SongError.NotFound(songId));
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
}
