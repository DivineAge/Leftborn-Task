
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Publisher.CreatePubliser;

public sealed record CreatePublisherCommand(Guid PublisherId, string FirstName, string LastName) : ICommand;
