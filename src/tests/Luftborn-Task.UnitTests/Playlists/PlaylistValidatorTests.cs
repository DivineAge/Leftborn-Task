using FluentValidation.TestHelper;
using Module.Playlist.Application.Playlist.CreatePlaylist;
using Module.Playlist.Application.Playlist.DeletePlaylist;
using Module.Playlist.Application.Playlist.UpdatePlaylist;
using Module.Playlist.Application.PlaylistSongs.AddSong;
using Module.Playlist.Application.PlaylistSongs.RemoveSong;
using Module.Playlist.Application.Songs.CreateSong;
using Module.Playlist.Application.Songs.DeleteSong;
using Module.Playlist.Application.Songs.UpdateSong;
using Module.Playlist.Application.User.CreateUser;
using Module.Playlist.Application.User.DeleteUser;
using Module.Playlist.Application.User.UpdateUser;
using Xunit;

namespace Luftborn_Task.UnitTests.Playlists;

public class PlaylistValidatorTests
{
    [Fact]
    public void CreatePlaylistCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new CreatePlaylistCommandValidator();
        var command = new CreatePlaylistCommand(Guid.Empty, "");

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreatePlaylistCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new CreatePlaylistCommandValidator();
        var command = new CreatePlaylistCommand(Guid.NewGuid(), "My Chill Beats");

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdatePlaylistCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new UpdatePlaylistCommandValidator();
        var command = new UpdatePlaylistCommand(Guid.Empty, "", Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
        result.ShouldHaveValidationErrorFor(x => x.Name);
        result.ShouldHaveValidationErrorFor(x => x.OwnerId);
    }

    [Fact]
    public void UpdatePlaylistCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new UpdatePlaylistCommandValidator();
        var command = new UpdatePlaylistCommand(Guid.NewGuid(), "Updated Playlist", Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DeletePlaylistValidator_ShouldHaveError_WhenIdIsEmpty()
    {
        // Arrange
        var validator = new DeletePlaylistValidator();
        var command = new DeletePlaylistCommand(Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PlaylistId);
    }

    [Fact]
    public void DeletePlaylistValidator_ShouldNotHaveError_WhenIdIsValid()
    {
        // Arrange
        var validator = new DeletePlaylistValidator();
        var command = new DeletePlaylistCommand(Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void AddSongCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new AddSongCommandValidator();
        var command = new AddSongCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PlaylistId);
        result.ShouldHaveValidationErrorFor(x => x.SongId);
        result.ShouldHaveValidationErrorFor(x => x.OwnerId);
    }

    [Fact]
    public void AddSongCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new AddSongCommandValidator();
        var command = new AddSongCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void RemoveSongCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new RemoveSongCommandValidator();
        var command = new RemoveSongCommand(Guid.Empty, Guid.Empty, Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PlaylistId);
        result.ShouldHaveValidationErrorFor(x => x.SongId);
        result.ShouldHaveValidationErrorFor(x => x.OwnerId);
    }

    [Fact]
    public void RemoveSongCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new RemoveSongCommandValidator();
        var command = new RemoveSongCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
