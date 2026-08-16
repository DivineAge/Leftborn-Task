
using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.User.DeleteUser;

internal sealed class DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        Domain.Users.User? user = await userRepository.GetAsync(command.Id, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserError.NotFound(command.Id));
        }

        userRepository.Delete(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

