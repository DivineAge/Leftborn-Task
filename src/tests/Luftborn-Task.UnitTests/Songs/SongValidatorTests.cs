using FluentValidation.TestHelper;
using Module.Songs.Application.Publisher.CreatePublisher;
using Module.Songs.Application.Publisher.DeletePublisher;
using Module.Songs.Application.Publisher.UpdatePublisher;
using Module.Songs.Application.Songs.CreateSong;
using Module.Songs.Application.Songs.DeleteSong;
using Module.Songs.Application.Songs.UpdateSong;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class SongValidatorTests
{
    [Fact]
    public void CreateSongCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new CreateSongCommandValidator();
        var command = new CreateSongCommand("", 0, Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
        result.ShouldHaveValidationErrorFor(x => x.TimeInSeconds);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void CreateSongCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new CreateSongCommandValidator();
        var command = new CreateSongCommand("Valid Song Name", 200, Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdateSongCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new UpdateSongCommandValidator();
        var command = new UpdateSongCommand(Guid.Empty, Guid.Empty, "", 0);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.SongId);
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
        result.ShouldHaveValidationErrorFor(x => x.TimeInSeconds);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void UpdateSongCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new UpdateSongCommandValidator();
        var command = new UpdateSongCommand(Guid.NewGuid(), Guid.NewGuid(), "Updated Name", 180);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DeleteSongCommandValidator_ShouldHaveError_WhenIdIsEmpty()
    {
        // Arrange
        var validator = new DeleteSongCommandValidator();
        var command = new DeleteSongCommand(Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void DeleteSongCommandValidator_ShouldNotHaveError_WhenIdIsValid()
    {
        // Arrange
        var validator = new DeleteSongCommandValidator();
        var command = new DeleteSongCommand(Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CreatePublisherCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new CreatePublisherCommandValidator();
        var command = new CreatePublisherCommand(Guid.Empty, "", "", "invalid-email");

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void CreatePublisherCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new CreatePublisherCommandValidator();
        var command = new CreatePublisherCommand(Guid.NewGuid(), "John", "Doe", "john@example.com");

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdatePublisherCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new UpdatePublisherCommandValidator();
        var command = new UpdatePublisherCommand(Guid.Empty, "", "");

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PublisherId);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void UpdatePublisherCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new UpdatePublisherCommandValidator();
        var command = new UpdatePublisherCommand(Guid.NewGuid(), "John", "Doe");

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DeletePublisherCommandValidator_ShouldHaveError_WhenIdIsEmpty()
    {
        // Arrange
        var validator = new DeletePublisherCommandValidator();
        var command = new DeletePublisherCommand(Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void DeletePublisherCommandValidator_ShouldNotHaveError_WhenIdIsValid()
    {
        // Arrange
        var validator = new DeletePublisherCommandValidator();
        var command = new DeletePublisherCommand(Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
