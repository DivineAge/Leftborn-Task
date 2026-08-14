using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Playlists;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.Playlist.CreatePlaylist;

internal sealed class CreatePlaylistCommandHandler(IPlaylistRepository playlistRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreatePlaylistCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
    {
        Domain.Playlists.Playlist playlist = Domain.Playlists.Playlist.Create(request.UserId, request.Name);

        playlistRepository.Insert(playlist);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(playlist.Id);
    }
}



