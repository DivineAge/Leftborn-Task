
using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Songs;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.Songs.DeleteSong;

internal sealed class DeleteSongCommandHandler(IUnitOfWork unitOfWork, ISongRepository repository) : ICommandHandler<DeleteSongCommand>
{
    public async Task<Result> Handle(DeleteSongCommand command, CancellationToken cancellationToken)
    {
        Song? song = await repository.GetAsync(command.Id, cancellationToken);

        if (song is null)
        {
            return Result.Failure(SongError.NotFound(command.Id));
        }

        repository.Delete(song);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

