using MediatR;
using Module.Playlist.PublicApi;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.UpdateSong;

internal sealed class UpdateSongCommandHandler(ISongRepository songRepository,
IPlaylistApi playlistApi,
IPublisherRepository publisherRepository,
 IUnitOfWork unitOfWork) : ICommandHandler<UpdateSongCommand>
{

    public async Task<Result> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        Song? song = await songRepository.GetAsync(request.SongId, cancellationToken);
        if (song is null)
        {
            return Result.Failure(SongError.NotFound(request.SongId));
        }

        Domain.Publisher.Publisher? publisher = await publisherRepository.GetAsync(request.PublisherId, cancellationToken);
        if (publisher is null)
        {
            return Result.Failure(PublisherErrors.NotFound(request.PublisherId));
        }

        await playlistApi.UpdateSongAsync(request.SongId, request.PublisherId, request.TimeInSeconds, request.Name, cancellationToken);

        song.Update(request.PublisherId, request.TimeInSeconds, request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
