

using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Users;
using Test.Common.Domain;
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.User.UpdatePublisher;

internal sealed class UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        Domain.Users.User? user = await userRepository.GetAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserError.NotFound(request.UserId));
        }

        user.Update(request.FirstName, request.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
