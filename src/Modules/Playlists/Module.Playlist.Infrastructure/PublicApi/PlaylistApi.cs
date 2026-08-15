using MediatR;
using Module.Playlist.Application.Songs.CreateSong;
using Module.Playlist.Application.User.CreateUser;
using Module.Playlist.PublicApi;
using Module.Playlist.Application.User.UpdateUser;
using Module.Playlist.Application.Songs.UpdateSong;

namespace Module.Playlist.Infrastructure.PublicApi
{
    public class PlaylistApi(ISender sender) : IPlaylistApi
    {
        public async Task CreateSongAsync(Guid songId, Guid publisherId, int timeInSeconds, string name, CancellationToken cancellationToken = default)
        {
            try
            {
                await sender.Send(new CreateSongCommand(songId, name, timeInSeconds, publisherId), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create song: {ex.Message}", ex);
            }
        }

        public async Task CreateUserAsync(Guid userId, string firstName, string lastName, string email, CancellationToken cancellationToken = default)
        {
            try
            {
                await sender.Send(new CreateUserCommand(userId, firstName, lastName, email), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create user: {ex.Message}", ex);
            }
        }

        public async Task UpdateSongAsync(Guid songId, Guid publisherId, int timeInSeconds, string name, CancellationToken cancellationToken = default)
        {
            try
            {
                await sender.Send(new UpdateSongCommand(songId, publisherId, name, timeInSeconds), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update song: {ex.Message}", ex);
            }
        }

        public async Task UpdateUserAsync(Guid userId, string firstName, string lastName, CancellationToken cancellationToken = default)
        {
            try
            {
                await sender.Send(new UpdateUserCommand(userId, firstName, lastName), cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update user: {ex.Message}", ex);
            }
        }
    }
}