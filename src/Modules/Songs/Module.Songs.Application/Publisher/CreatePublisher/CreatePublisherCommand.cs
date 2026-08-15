
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Publisher.CreatePublisher;

public sealed record CreatePublisherCommand(Guid PublisherId, string FirstName, string LastName, string Email) : ICommand;
