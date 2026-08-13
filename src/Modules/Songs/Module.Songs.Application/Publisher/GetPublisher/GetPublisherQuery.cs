

using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Publisher.GetPublisher;

public sealed record GetPublisherQuery(Guid PublisherId) : IQuery<PublisherResponse>;



