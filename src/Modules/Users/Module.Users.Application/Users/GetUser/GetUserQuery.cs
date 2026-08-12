using Test.Common.Application.Messaging;

namespace Modules.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid Id) : IQuery<UserResponse>;



