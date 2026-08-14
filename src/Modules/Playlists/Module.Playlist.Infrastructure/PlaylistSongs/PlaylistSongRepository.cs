
using Microsoft.EntityFrameworkCore;
using Module.Playlist.Domain.PlaylistSongs;
using Module.Playlist.Infrastructure.Database;

namespace Module.Playlist.Infrastructure.PlaylistSongs;

public class PlaylistSongRepository(PlaylistDbContext dbContext) : IPlaylistSongsRepository
{
    public void Delete(PlaylistSong playlistSong)
    {
        dbContext.PlaylistSongs.Remove(playlistSong);
    }

    public async Task<PlaylistSong?> GetAsync(Guid playlistId, Guid songId, CancellationToken cancellationToken)
    {
        return await dbContext.PlaylistSongs.FirstOrDefaultAsync(x => x.PlaylistId == playlistId && x.SongId == songId, cancellationToken);
    }

    public void Insert(PlaylistSong playlistSong)
    {
        dbContext.PlaylistSongs.Add(playlistSong);
    }
}
