
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.Playlist.CreatePlaylist;

public sealed record CreatePlaylistCommand(Guid UserId, string Name) : ICommand<Guid>;

