using MediatR;
using Module.Playlist.Application.Songs.CreateSong;
using Module.Playlist.Application.User.CreateUser;
using Module.Playlist.PublicApi;
using Module.Playlist.Application.User.UpdateUser;
using Module.Playlist.Application.Songs.UpdateSong;
using Module.Playlist.Application.User.DeleteUser;
using Module.Playlist.Application.Songs.DeleteSong;

namespace Module.Playlist.Infrastructure.PublicApi
{
    public class PlaylistApi(ISender sender) : IPlaylistApi
    {
        public async Task CreateSongAsync(Guid songId, Guid publisherId, int timeInSeconds, string name, CancellationToken cancellationToken = default)
        {

            await sender.Send(new CreateSongCommand(songId, name, timeInSeconds, publisherId), cancellationToken);

        }

        public async Task CreateUserAsync(Guid userId, string firstName, string lastName, string email, CancellationToken cancellationToken = default)
        {

            await sender.Send(new CreateUserCommand(userId, firstName, lastName, email), cancellationToken);


        }

        public async Task DeleteSongAsync(Guid songId, CancellationToken cancellationToken = default)
        {

            await sender.Send(new DeleteSongCommand(songId), cancellationToken);

        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {

            await sender.Send(new DeleteUserCommand(userId), cancellationToken);

        }

        public async Task UpdateSongAsync(Guid songId, Guid publisherId, int timeInSeconds, string name, CancellationToken cancellationToken = default)
        {

            await sender.Send(new UpdateSongCommand(songId, publisherId, name, timeInSeconds), cancellationToken);

        }

        public async Task UpdateUserAsync(Guid userId, string firstName, string lastName, CancellationToken cancellationToken = default)
        {

            await sender.Send(new UpdateUserCommand(userId, firstName, lastName), cancellationToken);

        }
    }
}