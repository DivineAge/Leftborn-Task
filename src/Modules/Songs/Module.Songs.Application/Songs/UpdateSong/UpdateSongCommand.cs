

using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Songs.UpdateSong;
public sealed  record UpdateSongCommand(Guid SongId , Guid PublisherId , string Name , int TimeInSeconds) : ICommand;

    

