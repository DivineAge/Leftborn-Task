using FluentValidation.TestHelper;
using Module.Users.Application.Users.DeleteUser;
using Module.Users.Application.Users.RegisterUser;
using Module.Users.Application.Users.UpdateUser;
using Xunit;

namespace Luftborn_Task.UnitTests.Users;

public class UserValidatorTests
{
    [Fact]
    public void RegisterUserCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new RegisterUserCommandValidator();
        var invalidCommand = new RegisterUserCommand("", "", "invalid-email");

        // Act & Assert
        var result = validator.TestValidate(invalidCommand);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void RegisterUserCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new RegisterUserCommandValidator();
        var validCommand = new RegisterUserCommand("John", "Doe", "john.doe@example.com");

        // Act & Assert
        var result = validator.TestValidate(validCommand);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void UpdateUserCommandValidator_ShouldHaveErrors_WhenFieldsAreInvalid()
    {
        // Arrange
        var validator = new UpdateUserCommandValidator();
        var invalidCommand = new UpdateUserCommand(Guid.Empty, "", "");

        // Act & Assert
        var result = validator.TestValidate(invalidCommand);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }

    [Fact]
    public void UpdateUserCommandValidator_ShouldNotHaveErrors_WhenCommandIsValid()
    {
        // Arrange
        var validator = new UpdateUserCommandValidator();
        var validCommand = new UpdateUserCommand(Guid.NewGuid(), "Jane", "Smith");

        // Act & Assert
        var result = validator.TestValidate(validCommand);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void DeleteUserCommandValidator_ShouldHaveError_WhenIdIsEmpty()
    {
        // Arrange
        var validator = new DeleteUserCommandValidator();
        var invalidCommand = new DeleteUserCommand(Guid.Empty);

        // Act & Assert
        var result = validator.TestValidate(invalidCommand);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void DeleteUserCommandValidator_ShouldNotHaveError_WhenIdIsValid()
    {
        // Arrange
        var validator = new DeleteUserCommandValidator();
        var validCommand = new DeleteUserCommand(Guid.NewGuid());

        // Act & Assert
        var result = validator.TestValidate(validCommand);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
