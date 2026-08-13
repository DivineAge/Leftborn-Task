

namespace Module.Songs.Application.Publisher.GetPublisher
;

internal sealed record PublisherResponse
(
    Guid PublisherId,
    string FirstName,
    string LastName
);
