using Module.Users.Domain.Users;
using Xunit;

namespace Luftborn_Task.UnitTests.Users;

public class UserDomainTests
{
    [Fact]
    public void Create_ShouldSetProperties_WhenCalled()
    {
        // Act
        var user = User.Create("John", "Doe", "john@example.com");

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("john@example.com", user.Email);
    }

    [Fact]
    public void Update_ShouldChangeProperties_WhenValuesAreNew()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@example.com");

        // Act
        user.Update("Jane", "Smith");

        // Assert
        Assert.Equal("Jane", user.FirstName);
        Assert.Equal("Smith", user.LastName);
    }

    [Fact]
    public void Update_ShouldNotChangeProperties_WhenValuesAreSame()
    {
        // Arrange
        var user = User.Create("John", "Doe", "john@example.com");

        // Act
        user.Update("John", "Doe");

        // Assert
        Assert.Equal("John", user.FirstName);
        Assert.Equal("Doe", user.LastName);
    }
}
