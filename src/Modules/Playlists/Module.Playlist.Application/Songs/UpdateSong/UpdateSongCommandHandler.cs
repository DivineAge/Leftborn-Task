using MediatR;
using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Songs;
using Module.Playlist.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.Songs.UpdateSong;

internal sealed  class UpdateSongCommandHandler(ISongRepository songRepository,IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateSongCommand>
{

    public async Task<Result> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
    {
        Song? song = await songRepository.GetAsync(request.SongId, cancellationToken);

        if (song is null)
        {
            return Result.Failure(SongError.NotFound(request.SongId));
        }
        Domain.Users.User? user = await userRepository.GetAsync(request.PublisherId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserError.NotFound(request.PublisherId));
        }

        song.Update(request.PublisherId, request.TimeInSeconds, request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
