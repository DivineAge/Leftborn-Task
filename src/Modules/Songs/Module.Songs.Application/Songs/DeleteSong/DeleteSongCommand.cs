

using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Songs.DeleteSong;

public sealed record DeleteSongCommand(Guid Id) : ICommand;



