
using Test.Common.Domain;

namespace Module.Playlist.Domain.PlaylistSongs;

public static class PlaylistSongsErrors
{
    public static Error NotFound(Guid songId, Guid playlistId) => Error.NotFound("Song.NotFound", $"Song with id: {songId} not found in playlist: {playlistId}");


}
