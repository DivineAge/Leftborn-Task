using Test.Common.Domain;

namespace Module.Songs.Domain.Publisher;

public class PublisherErrors
{
    public static Error NotFound(Guid publisherId) => Error.NotFound("Publisher.NotFound", $"The publisher with the identifier {publisherId} not found");
    public static Error PublisherEmailAlreadyExists(string email) => Error.Conflict("Publisher.EmailAlreadyExists", $"The publisher with the email {email} already exists");

}