
using Module.Songs.PublicApi;
using Module.Users.Application.Abstractions.Data;
using Module.Users.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Users.Application.Users.RegisterUser;

public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, ISongsApi publicApi, IUserRepository userRepository) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {

        var user = User.Create(command.FirstName, command.LastName);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await publicApi.CreatePublisherAsync(user.Id, user.FirstName, user.LastName, cancellationToken);

        return user.Id;
    }
}
