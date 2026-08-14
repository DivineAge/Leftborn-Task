

using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.PlaylistSongs.AddSong;

public sealed record class AddSongCommand(Guid PlaylistId, Guid SongId, Guid OwnerId) : ICommand;



