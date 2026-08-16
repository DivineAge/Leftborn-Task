


using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.User.DeleteUser;

public sealed record class DeleteUserCommand(Guid Id) : ICommand;



