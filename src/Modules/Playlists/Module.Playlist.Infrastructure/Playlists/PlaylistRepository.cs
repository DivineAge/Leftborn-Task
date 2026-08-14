using Microsoft.EntityFrameworkCore;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Infrastructure.Database;

namespace Module.Playlist.Infrastructure.Playlists;

public class PlaylistRepository(PlaylistDbContext dbContext) : IPlaylistRepository
{
    public void Delete(Domain.Playlists.Playlist playlist)
    {
        dbContext.Playlists.Remove(playlist);
    }

    public async Task<Domain.Playlists.Playlist?> GetAsync(Guid playlistId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Playlists.SingleOrDefaultAsync(x => x.Id == playlistId, cancellationToken);
    }

    public void Insert(Domain.Playlists.Playlist playlist)
    {
        dbContext.Playlists.Add(playlist);
    }
}
