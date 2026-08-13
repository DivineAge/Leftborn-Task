using Test.Common.Application.Messaging;


namespace Module.Songs.Application.Publisher.RegisterPublisher;

internal sealed record CreatePublisherCommand(Guid PublisherId, string FirstName, string LastName) : ICommand;



