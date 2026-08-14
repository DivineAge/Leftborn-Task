
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.User.CreateUser;

public sealed record CreateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand;
