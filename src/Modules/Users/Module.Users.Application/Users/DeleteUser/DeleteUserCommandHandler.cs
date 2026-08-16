
using Module.Playlist.PublicApi;
using Module.Songs.PublicApi;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Users.Application.Users.DeleteUser;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    ISongsApi songsApi,
    IPlaylistApi playlistsApi,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        
        User? user = await userRepository.GetAsync(command.Id, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.Id));
        }


            await songsApi.DeletePublisherAsync(user.Id, cancellationToken);

            await playlistsApi.DeleteUserAsync(user.Id, cancellationToken);

            userRepository.Delete(user);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
      
    }
}

