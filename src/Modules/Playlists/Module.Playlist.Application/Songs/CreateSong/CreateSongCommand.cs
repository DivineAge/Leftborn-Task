

using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.Songs.CreateSong;

public sealed record CreateSongCommand(Guid SongId, string Name, int TimeInSeconds, Guid PublisherId) : ICommand;



