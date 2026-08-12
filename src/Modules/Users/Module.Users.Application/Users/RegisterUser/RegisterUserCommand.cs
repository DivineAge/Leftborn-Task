using Test.Common.Application.Messaging;


namespace Module.Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(string FirstName, string LastName) : ICommand<Guid>;



