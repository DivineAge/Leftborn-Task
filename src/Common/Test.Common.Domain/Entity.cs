namespace Test.Common.Domain;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected Entity()
    {
    }
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();

    public void Raise(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
