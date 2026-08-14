


using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.Playlist.DeletePlaylist;

public sealed record DeletePlaylistCommand(Guid PlaylistId, Guid OwnerId) : ICommand;

