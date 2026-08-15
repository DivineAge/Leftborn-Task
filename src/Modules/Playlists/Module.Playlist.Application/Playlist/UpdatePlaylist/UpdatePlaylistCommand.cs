
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.Playlist.UpdatePlaylist;

public sealed  record UpdatePlaylistCommand(Guid Id, string Name, Guid OwnerId): ICommand;

    

