
using Test.Common.Domain;

namespace Module.Songs.Domain.Songs;

public sealed class SongDeletedDomainEvent(Guid id) : DomainEvent
{
    public Guid SongId { get; init; } = id;
}


