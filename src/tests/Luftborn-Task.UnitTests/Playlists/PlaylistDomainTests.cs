using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.PlaylistSongs;
using Xunit;

namespace Luftborn_Task.UnitTests.Playlists;

public class PlaylistDomainTests
{
    [Fact]
    public void Playlist_Create_ShouldSetProperties_WhenCalled()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        // Act
        var playlist = Playlist.Create(ownerId, "My Favorite Songs");

        // Assert
        Assert.NotEqual(Guid.Empty, playlist.Id);
        Assert.Equal(ownerId, playlist.OwnerId);
        Assert.Equal("My Favorite Songs", playlist.Name);
    }

    [Fact]
    public void Playlist_Update_ShouldChangeProperties_WhenValuesAreNew()
    {
        // Arrange
        var initialOwnerId = Guid.NewGuid();
        var newOwnerId = Guid.NewGuid();
        var playlist = Playlist.Create(initialOwnerId, "Old Name");

        // Act
        playlist.Update("New Name", newOwnerId);

        // Assert
        Assert.Equal(newOwnerId, playlist.OwnerId);
        Assert.Equal("New Name", playlist.Name);
    }

    [Fact]
    public void Playlist_Update_ShouldNotChangeProperties_WhenValuesAreSame()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var playlist = Playlist.Create(ownerId, "Same Name");

        // Act
        playlist.Update("Same Name", ownerId);

        // Assert
        Assert.Equal(ownerId, playlist.OwnerId);
        Assert.Equal("Same Name", playlist.Name);
    }

    [Fact]
    public void PlaylistSong_Create_ShouldSetProperties_WhenCalled()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var songId = Guid.NewGuid();

        // Act
        var playlistSong = PlaylistSong.Create(playlistId, songId);

        // Assert
        Assert.Equal(playlistId, playlistSong.PlaylistId);
        Assert.Equal(songId, playlistSong.SongId);
    }

    [Fact]
    public void PlaylistSong_Update_ShouldChangeProperties_WhenValuesAreNew()
    {
        // Arrange
        var playlistSong = PlaylistSong.Create(Guid.NewGuid(), Guid.NewGuid());
        var newPlaylistId = Guid.NewGuid();
        var newSongId = Guid.NewGuid();

        // Act
        playlistSong.Update(newPlaylistId, newSongId);

        // Assert
        Assert.Equal(newPlaylistId, playlistSong.PlaylistId);
        Assert.Equal(newSongId, playlistSong.SongId);
    }

    [Fact]
    public void PlaylistSong_Update_ShouldNotChangeProperties_WhenValuesAreSame()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var songId = Guid.NewGuid();
        var playlistSong = PlaylistSong.Create(playlistId, songId);

        // Act
        playlistSong.Update(playlistId, songId);

        // Assert
        Assert.Equal(playlistId, playlistSong.PlaylistId);
        Assert.Equal(songId, playlistSong.SongId);
    }
}
