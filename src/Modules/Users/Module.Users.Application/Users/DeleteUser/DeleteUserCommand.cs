using Test.Common.Application.Messaging;

namespace Module.Users.Application.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : ICommand;


