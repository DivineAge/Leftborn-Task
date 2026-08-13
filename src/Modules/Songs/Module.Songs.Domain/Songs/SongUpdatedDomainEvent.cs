

using Test.Common.Domain;

namespace Module.Songs.Domain.Songs;

public sealed class SongUpdatedDomainEvent(Guid id, string name, int timeInSeconds) : DomainEvent
{
    public Guid SongId { get; init; } = id;
    public string Name { get; init; } = name;
    public int TimeInSeconds { get; init; } = timeInSeconds;
}
