using Test.Common.Domain;

namespace Module.Songs.Domain.Publisher;

public class PublisherError
{
    public static Error NotFound(Guid publisherId) => Error.NotFound("Publisher.NotFound", $"The publisher with the identifier {publisherId} not found");

}