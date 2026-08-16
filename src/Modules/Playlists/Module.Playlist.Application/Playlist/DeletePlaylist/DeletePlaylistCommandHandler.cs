

using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.Playlist.DeletePlaylist;

internal sealed class DeletePlaylistCommandHandler(
    IPlaylistRepository playlistRepository,
    IUnitOfWork unitOfWork)
 : ICommandHandler<DeletePlaylistCommand>
{
    public async Task<Result> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
    {
        Domain.Playlists.Playlist? playlist = await playlistRepository.GetAsync(request.PlaylistId, cancellationToken);

        if (playlist is null)
        {
            return Result.Failure(PlaylistErrors.NotFound(request.PlaylistId));
        }

        playlistRepository.Delete(playlist);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
