

using Module.Playlist.Domain.Songs;
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.PlaylistSongs.GetUserPlaylist;

public sealed record GetUserPlaylistQuery(Guid UserId, Guid PlaylistId) : IQuery<IEnumerable<UserPlaylistResponse>>;



