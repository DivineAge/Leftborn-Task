using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class SongDomainTests
{
    [Fact]
    public void Song_Create_ShouldSetProperties_WhenCalled()
    {
        // Arrange
        var publisherId = Guid.NewGuid();

        // Act
        var song = Song.Create(publisherId, 240, "Bohemian Rhapsody");

        // Assert
        Assert.NotEqual(Guid.Empty, song.Id);
        Assert.Equal(publisherId, song.PublisherId);
        Assert.Equal(240, song.TimeInSeconds);
        Assert.Equal("Bohemian Rhapsody", song.Name);
    }

    [Fact]
    public void Song_Update_ShouldChangeProperties_WhenValuesAreNew()
    {
        // Arrange
        var initialPublisherId = Guid.NewGuid();
        var newPublisherId = Guid.NewGuid();
        var song = Song.Create(initialPublisherId, 240, "Bohemian Rhapsody");

        // Act
        song.Update(newPublisherId, 300, "Updated Song");

        // Assert
        Assert.Equal(newPublisherId, song.PublisherId);
        Assert.Equal(300, song.TimeInSeconds);
        Assert.Equal("Updated Song", song.Name);
    }

    [Fact]
    public void Song_Update_ShouldNotChangeProperties_WhenValuesAreSame()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var song = Song.Create(publisherId, 240, "Bohemian Rhapsody");

        // Act
        song.Update(publisherId, 240, "Bohemian Rhapsody");

        // Assert
        Assert.Equal(publisherId, song.PublisherId);
        Assert.Equal(240, song.TimeInSeconds);
        Assert.Equal("Bohemian Rhapsody", song.Name);
    }

    [Fact]
    public void Publisher_Create_ShouldSetProperties_WhenCalled()
    {
        // Arrange
        var publisherId = Guid.NewGuid();

        // Act
        var publisher = Publisher.Create(publisherId, "John", "Lennon", "john@example.com");

        // Assert
        Assert.Equal(publisherId, publisher.Id);
        Assert.Equal("John", publisher.FirstName);
        Assert.Equal("Lennon", publisher.LastName);
        Assert.Equal("john@example.com", publisher.Email);
    }

    [Fact]
    public void Publisher_Update_ShouldChangeProperties_WhenValuesAreNew()
    {
        // Arrange
        var publisher = Publisher.Create(Guid.NewGuid(), "John", "Lennon", "john@example.com");

        // Act
        publisher.Update("Paul", "McCartney");

        // Assert
        Assert.Equal("Paul", publisher.FirstName);
        Assert.Equal("McCartney", publisher.LastName);
    }

    [Fact]
    public void Publisher_Update_ShouldNotChangeProperties_WhenValuesAreSame()
    {
        // Arrange
        var publisher = Publisher.Create(Guid.NewGuid(), "John", "Lennon", "john@example.com");

        // Act
        publisher.Update("John", "Lennon");

        // Assert
        Assert.Equal("John", publisher.FirstName);
        Assert.Equal("Lennon", publisher.LastName);
    }
}
