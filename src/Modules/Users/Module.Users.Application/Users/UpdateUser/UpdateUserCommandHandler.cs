using Test.Common.Application.Messaging;
using Test.Common.Domain;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Domain.Users;
using Module.Songs.PublicApi;
using Module.Playlist.PublicApi;

namespace Module.Users.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
     ISongsApi publicApi,
      IPlaylistApi playlistApi,
       IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserError.NotFound(request.UserId));
        }

        user.Update(request.FirstName, request.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publicApi.UpdatePublisherAsync(user.Id, user.FirstName, user.LastName, cancellationToken);
        await  playlistApi.UpdateUserAsync(user.Id, user.FirstName, user.LastName, cancellationToken);

        return Result.Success();
    }
}
