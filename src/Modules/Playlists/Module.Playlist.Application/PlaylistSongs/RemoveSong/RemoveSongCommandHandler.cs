

using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Playlists;
using Module.Playlist.Domain.PlaylistSongs;
using Module.Playlist.Domain.Songs;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.PlaylistSongs.RemoveSong;

internal sealed class RemoveSongCommandHandler(
    IPlaylistSongsRepository playlistSongsRepository,
    ISongRepository songRepository,
    IPlaylistRepository playlistRepository,

    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveSongCommand>
{
    public async Task<Result> Handle(RemoveSongCommand request, CancellationToken cancellationToken)
    {

        Song? song = await songRepository.GetAsync(request.SongId, cancellationToken);

        if (song is null)
        {
            return Result.Failure(SongError.NotFound(request.SongId));
        }

        Domain.Playlists.Playlist? playlist = await playlistRepository.GetAsync(request.PlaylistId, cancellationToken);

        if (playlist is null)
        {
            return Result.Failure(PlaylistErrors.NotFound(request.PlaylistId));
        }

        if (playlist.OwnerId != request.OwnerId)
        {
            return Result.Failure(PlaylistErrors.NotPlaylistOwner(request.OwnerId, request.PlaylistId));
        }
        PlaylistSong? playListSong = await playlistSongsRepository.GetAsync(request.PlaylistId, request.SongId, cancellationToken);

        if (playListSong is null)
        {
            return Result.Failure(PlaylistSongsErrors.NotFound(request.SongId, request.PlaylistId));
        }

        PlaylistSong playlistSong = PlaylistSong.Create(request.PlaylistId, request.SongId);

        playlistSongsRepository.Delete(playlistSong);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}