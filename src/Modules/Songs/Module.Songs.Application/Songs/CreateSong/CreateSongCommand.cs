

using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Songs.CreateSong;

public sealed record CreateSongCommand(string Name, int TimeInSeconds, Guid PublisherId) : ICommand<Guid>;



