

using Test.Common.Domain;

namespace Module.Playlist.Domain.Playlists;

public class PlaylistErrors

{
    public static Error NotFound(Guid playlistId) => Error.NotFound("Playlist.NotFound", $"The playlist with the identifier {playlistId} not found");

    public static Error NotFoundByName(string name) => Error.NotFound("Playlist.NameNotFound", $"The playlist with the name {name} not found");

    public static Error NotPlaylistOwner(Guid userId, Guid playlistId) => Error.Problem("Playlist.NotOwner", $"The user with the identifier {userId} is not the owner of the playlist with the identifier {playlistId}");
}
