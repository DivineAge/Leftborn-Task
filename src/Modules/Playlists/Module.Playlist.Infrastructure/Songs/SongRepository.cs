

using Microsoft.EntityFrameworkCore;
using Module.Playlist.Domain.Songs;
using Module.Playlist.Infrastructure.Database;

namespace Module.Playlist.Infrastructure.Songs;

public class SongRepository(PlaylistDbContext dbContext) : ISongRepository
{
    public void Delete(Song song)
    {
        dbContext.Songs.Remove(song);
    }

    public async Task<Song?> GetAsync(Guid songId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Songs.SingleOrDefaultAsync(x => x.Id == songId, cancellationToken);
    }

    public void Insert(Song song)
    {
        dbContext.Songs.Add(song);
    }
}
