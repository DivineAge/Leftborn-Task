using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module.Playlist.PublicApi;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Songs;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.DeleteSong;

internal sealed class DeleteSongCommandHandler(IPlaylistApi playlistApi, ISongRepository songRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteSongCommand>
{
    public async Task<Result> Handle(DeleteSongCommand command, CancellationToken cancellationToken)
    {
        var song = await songRepository.GetAsync(command.Id, cancellationToken);

        if (song is null)
        {
            return Result.Failure(SongErrors.NotFound(command.Id));
        }



            await playlistApi.DeleteSongAsync(command.Id, cancellationToken);

            songRepository.Delete(song);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
    
    }

}
