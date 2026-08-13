
using Test.Common.Domain;

namespace Module.Songs.Domain.Songs;

public sealed class Song : Entity
{
    private Song()
    {
    }
    public Guid Id { get; private set; }

    public Guid PublisherId { get; private set; }

    public int TimeInSeconds { get; private set; }

    public string Name { get; private set; } = null!;

    public static Song Create(Guid publisherId, int timeInSeconds, string name)
    {
        var song = new Song
        {
            Id = Guid.NewGuid(),
            PublisherId = publisherId,
            TimeInSeconds = timeInSeconds,
            Name = name
        };
        song.Raise(new SongCreatedDomainEvent(song.Id));
        return song;
    }
    public void Update(int timeInSeconds, string name)
    {
        if (TimeInSeconds == timeInSeconds && Name == name)
        {
            return;
        }
        TimeInSeconds = timeInSeconds;
        Name = name;
        Raise(new SongUpdatedDomainEvent(this.Id, name, timeInSeconds));
    }

    public void Delete()
    {
        Raise(new SongDeletedDomainEvent(this.Id));
    }
}
