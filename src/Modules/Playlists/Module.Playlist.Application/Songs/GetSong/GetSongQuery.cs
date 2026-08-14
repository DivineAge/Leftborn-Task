
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.Songs.GetSong;

public sealed record GetSongQuery(Guid Id) : IQuery<SongResponse>;
