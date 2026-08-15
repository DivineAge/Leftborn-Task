using Module.Songs.Application.Songs.GetSong;
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Songs.GetAllSongs;

public sealed record GetAllSongsQuery(): IQuery<IEnumerable<SongResponse>>;

    

