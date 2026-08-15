
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.User.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand;



