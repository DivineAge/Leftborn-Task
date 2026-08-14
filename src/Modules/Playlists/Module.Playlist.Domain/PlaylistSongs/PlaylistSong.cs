

namespace Module.Playlist.Domain.PlaylistSongs;

public sealed class PlaylistSong
{
    private PlaylistSong()
    {
    }

    public Guid PlaylistId { get; private set; }
    public Guid SongId { get; private set; }

    public static PlaylistSong Create(Guid playlistId, Guid songId)
    {
        PlaylistSong playlistSong = new()
        {
            PlaylistId = playlistId,
            SongId = songId
        };

        return playlistSong;
    }


}
