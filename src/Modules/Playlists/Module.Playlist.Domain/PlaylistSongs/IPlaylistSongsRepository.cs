

namespace Module.Playlist.Domain.PlaylistSongs;

public interface IPlaylistSongsRepository
{
    Task<PlaylistSong?> GetAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken);
    void Insert(PlaylistSong playlistSong);

    void Delete(PlaylistSong playlistSong);

}
