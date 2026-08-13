
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.CreateSong;

internal sealed class CreateSongCommandHandler(ISongRepository songRepository, IPublisherRepository publisherRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateSongCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        Domain.Publisher.Publisher? publisher = await publisherRepository.GetAsync(request.PublisherId, cancellationToken);
        if (publisher is null)
        {
            return Result.Failure<Guid>(PublisherError.NotFound(request.PublisherId));
        }
        var song = Song.Create(request.PublisherId, request.TimeInSeconds, request.Name);

        songRepository.Insert(song);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return song.Id;
    }

}
