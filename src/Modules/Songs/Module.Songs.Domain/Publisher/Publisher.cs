using Test.Common.Domain;

namespace Module.Songs.Domain.Publisher;

public sealed class Publisher
{

    private Publisher()
    {

    }
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }

    public static Publisher Create(Guid id, string firstName, string lastName, string Email)
    {
        return new Publisher
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Email = Email
        };
    }
    public void Update(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return;
        }
        FirstName = firstName;
        LastName = lastName;
    }
}
