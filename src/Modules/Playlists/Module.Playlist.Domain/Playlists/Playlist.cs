using Module.Playlist.Domain.Songs;
namespace Module.Playlist.Domain.Playlists;

public sealed class Playlist
{
    private Playlist()
    {
    }

    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public string Name { get; private set; }


    public static Playlist Create(Guid OwnerId, string name)
    {
        Playlist playlist = new()
        {
            Id = Guid.NewGuid(),
            OwnerId = OwnerId,
            Name = name
        };

        return playlist;
    }

}
