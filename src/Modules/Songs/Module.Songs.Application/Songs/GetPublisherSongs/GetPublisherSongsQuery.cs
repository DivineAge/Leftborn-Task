

using Module.Songs.Application.Songs.GetSong;
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Songs.GetPublisherSongs;

public sealed record GetPublisherSongsQuery(Guid PublisherId) : IQuery<IEnumerable<SongResponse>>;



