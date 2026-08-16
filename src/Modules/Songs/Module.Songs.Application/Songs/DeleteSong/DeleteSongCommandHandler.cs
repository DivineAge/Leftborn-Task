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
            return Result.Failure(SongError.NotFound(command.Id));
        }

        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            await playlistApi.DeleteSongAsync(command.Id, cancellationToken);

            songRepository.Delete(song);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

}
