using Test.Common.Domain;

namespace Test.Common.Application.Messaging;

public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public abstract Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
    public Task Handle(IDomainEvent domainEvent, CancellationToken cancellationtoken = default)
    {
        return Handle((TDomainEvent)domainEvent, cancellationtoken);
    }

}
