

using Test.Common.Domain;

namespace Module.Songs.Domain.Songs;

public static class SongError
{
    public static Error NotFound(Guid songId) => Error.NotFound("Song.NotFound", $"Song with id: {songId} not found");



}
