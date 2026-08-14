
namespace Module.Playlist.Domain.Playlists;

public interface IPlaylistRepository
{
    Task<Playlist?> GetAsync(Guid playlistId, CancellationToken cancellationToken = default);

    void Insert(Playlist playlist);

    void Delete(Playlist playlist);
}
