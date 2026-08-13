

using Microsoft.EntityFrameworkCore;
using Module.Songs.Domain.Songs;
using Module.Songs.Infrastructure.Database;

namespace Module.Songs.Infrastructure.Songs;

public class SongRepository(SongsDbContext dbContext) : ISongRepository
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
