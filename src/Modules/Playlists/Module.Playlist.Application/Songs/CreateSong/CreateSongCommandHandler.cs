
using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Users;
using Module.Playlist.Domain.Songs;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.Songs.CreateSong;

internal sealed class CreateSongCommandHandler(ISongRepository songRepository, IUserRepository userepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateSongCommand>
{
    public async Task<Result> Handle(CreateSongCommand request, CancellationToken cancellationToken)
    {
        Domain.Users.User? user = await userepository.GetAsync(request.PublisherId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(UserError.NotFound(request.PublisherId));
        }
        Song song = Song.Create(request.SongId, request.PublisherId, request.TimeInSeconds, request.Name);

        songRepository.Insert(song);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }

}
