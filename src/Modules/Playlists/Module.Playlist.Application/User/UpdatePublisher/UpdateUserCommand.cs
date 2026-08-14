
using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.User.UpdatePublisher;

public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName) : ICommand;



