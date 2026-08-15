using Test.Common.Domain;

namespace Module.Playlist.Domain.Users;

public sealed class User 
{

    private User()
    {

    }
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public static User Create(Guid id, string firstName, string lastName)
    {
        return new User
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName
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
