

using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.PlaylistSongs.RemoveSong;

public sealed record RemoveSongCommand(Guid PlaylistId, Guid SongId, Guid OwnerId) : ICommand;



