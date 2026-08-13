

using Test.Common.Domain;

namespace Module.Users.Domain.Users;

public sealed class UserRegisterDomainEvent(Guid Id) : DomainEvent
{
    public Guid UserId { get; init; } = Id;

}
