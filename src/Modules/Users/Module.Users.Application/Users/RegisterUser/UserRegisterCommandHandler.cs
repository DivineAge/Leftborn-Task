using Modules.Users.Application.Abstractions.Data;
using Modules.Users.Domain.Users;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Modules.Users.Application.Users.RegisterUser;

public class UserRegisterCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {

        var user = User.Create(command.FirstName, command.LastName);

        userRepository.Insert(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
