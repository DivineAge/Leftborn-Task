

using Test.Common.Domain;

namespace Module.Songs.Domain.Songs;

public static class SongErrors
{
    public static Error NotFound(Guid songId) => Error.NotFound("Song.NotFound", $"Song with id: {songId} not found");
    public static Error NotFound(string name) => Error.NotFound("Song.NotFound", $"Song with name: {name} not found");



}
