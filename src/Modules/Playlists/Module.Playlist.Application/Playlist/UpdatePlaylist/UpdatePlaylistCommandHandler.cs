using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.Playlist.UpdatePlaylist;

internal sealed class UpdatePlaylistCommandHandler(
    IPlaylistRepository playlistRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
: ICommandHandler<UpdatePlaylistCommand>
{
    public async Task<Result> Handle(UpdatePlaylistCommand request, CancellationToken cancellationToken)
    {
        Domain.Playlists.Playlist? playlist = await playlistRepository.GetAsync(request.Id, cancellationToken);

        if (playlist is null)
        {
            return Result.Failure(PlaylistErrors.NotFound(request.Id));
        }

        Domain.Users.User? user = await userRepository.GetAsync(request.OwnerId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserError.NotFound(request.OwnerId));
        }

        playlist.Update(request.Name, request.OwnerId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
