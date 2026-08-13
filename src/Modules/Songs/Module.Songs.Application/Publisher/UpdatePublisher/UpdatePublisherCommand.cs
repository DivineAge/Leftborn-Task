
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Publisher.UpdatePublisher;

public sealed record UpdatePublisherCommand(Guid PublisherId, string FirstName, string LastName) : ICommand;



