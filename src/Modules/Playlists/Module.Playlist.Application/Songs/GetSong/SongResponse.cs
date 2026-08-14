

namespace Module.Playlist.Application.Songs.GetSong;

public sealed record class SongResponse(Guid Id, string Name, int TimeInSeconds, Guid PublisherId);



