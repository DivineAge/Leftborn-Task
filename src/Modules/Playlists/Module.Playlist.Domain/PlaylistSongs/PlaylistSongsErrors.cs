
using Test.Common.Domain;

namespace Module.Playlist.Domain.PlaylistSongs;

public static class PlaylistSongsErrors
{
    public static Error NotFound(Guid songId, Guid playlistId) => Error.NotFound("Song.NotFound", $"Song with id: {songId} not found in playlist: {playlistId}");

    public static Error AlreadyExists(Guid playlistId, Guid songId) => Error.Conflict("Song.AlreadyExists", $"Song with id: {songId} already exists in playlist: {playlistId}");


}
