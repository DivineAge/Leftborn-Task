using Module.Playlist.Application.Abstractions.Data;
using Module.Playlist.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.User.CreateUser;

internal sealed class CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateUserCommand>
{
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Domain.Users.User? emailExists = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (emailExists is not null)
        {
            return Result.Failure(UserError.EmailAlreadyExists(request.Email));
        }
        Domain.Users.User user = Domain.Users.User.Create(request.UserId, request.FirstName, request.LastName, request.Email);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
