
using Test.Common.Domain;

namespace Module.Playlist.Domain.Songs;

public sealed class Song
{
    private Song()
    {
    }
    public Guid Id { get; private set; }

    public Guid PublisherId { get; private set; }

    public int TimeInSeconds { get; private set; }

    public string Name { get; private set; } = null!;

    public static Song Create(Guid id, Guid publisherId, int timeInSeconds, string name)
    {
        var song = new Song
        {
            Id = id,
            PublisherId = publisherId,
            TimeInSeconds = timeInSeconds,
            Name = name
        };

        return song;
    }


}
