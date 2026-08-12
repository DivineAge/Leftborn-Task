using Test.Common.Application.Messaging;

namespace Module.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid Id) : IQuery<UserResponse>;



