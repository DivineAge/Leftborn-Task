using Test.Common.Domain;

namespace Module.Users.Domain.Users;

public sealed class User
{

    private User()
    {

    }
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public static User Create(string firstName, string lastName)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName
        };
        return user;
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
