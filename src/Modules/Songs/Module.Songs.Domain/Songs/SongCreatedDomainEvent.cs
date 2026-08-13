

using Test.Common.Domain;

namespace Module.Songs.Domain.Songs;

public sealed class SongCreatedDomainEvent(Guid id) : DomainEvent
{
    public Guid UserId { get; init; } = id;
}
