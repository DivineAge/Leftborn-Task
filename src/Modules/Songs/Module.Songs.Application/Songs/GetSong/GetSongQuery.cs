
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Songs.GetSong;

public sealed record GetSongQuery(Guid Id) : IQuery<SongResponse>;
