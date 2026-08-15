

using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.Songs.UpdateSong;
public sealed  record UpdateSongCommand(Guid SongId , Guid PublisherId, string Name , int TimeInSeconds) : ICommand;

    

