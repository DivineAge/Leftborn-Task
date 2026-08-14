

namespace Module.Playlist.Application.PlaylistSongs.GetUserPlaylist;

public sealed record UserPlaylistResponse(
    Guid SongId,
    string Name,
    int TimeInSeconds,
    Guid PublisherId
);

