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

    public static Song Create(Guid songId, Guid publisherId, int timeInSeconds, string name)
    {
        var song = new Song
        {
            Id = songId,
            PublisherId = publisherId,
            TimeInSeconds = timeInSeconds,
            Name = name
        };

        return song;
    }
    public void Update(Guid publisherId, int timeInSeconds, string name)
    {
        if (TimeInSeconds == timeInSeconds && Name == name && PublisherId == publisherId)
        {
            return;
        }
        TimeInSeconds = timeInSeconds;
        Name = name;
        PublisherId = publisherId;
    }
}
