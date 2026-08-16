using Module.Songs.Application.Songs.GetSong;
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Songs.GetSongByName;

public sealed record GetSongByNameQuery(string Name) : IQuery<IEnumerable<SongResponse>>;

