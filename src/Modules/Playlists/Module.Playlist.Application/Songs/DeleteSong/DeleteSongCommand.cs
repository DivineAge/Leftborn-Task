

using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.Songs.DeleteSong;


public sealed record DeleteSongCommand(Guid Id) : ICommand;





