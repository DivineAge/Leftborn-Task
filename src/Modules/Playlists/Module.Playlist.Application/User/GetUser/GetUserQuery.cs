

using Test.Common.Application.Messaging;

namespace Module.Playlist.Application.User.GetUser;

public sealed record GetUserQuery(Guid UserId) : IQuery<UserResponse>;



