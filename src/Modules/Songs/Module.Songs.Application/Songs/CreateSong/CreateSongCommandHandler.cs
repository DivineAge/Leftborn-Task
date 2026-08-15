
using Module.Playlist.PublicApi;
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.CreateSong;

internal sealed class CreateSongCommandHandler(ISongRepository songRepository, IPlaylistApi playlistApi, IPublisherRepository publisherRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateSongCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        Domain.Publisher.Publisher? publisher = await publisherRepository.GetAsync(request.PublisherId, cancellationToken);
        if (publisher is null)
        {
            return Result.Failure<Guid>(PublisherError.NotFound(request.PublisherId));
        }
        Song song = Song.Create(request.PublisherId, request.TimeInSeconds, request.Name);

        
        

        try
        {      
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            songRepository.Insert(song);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await playlistApi.CreateSongAsync(song.Id, request.PublisherId, request.TimeInSeconds, request.Name, cancellationToken);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return song.Id;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

}
