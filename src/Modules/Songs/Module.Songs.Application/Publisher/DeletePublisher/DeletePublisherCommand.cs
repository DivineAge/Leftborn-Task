using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Publisher.DeletePublisher;

public sealed record DeletePublisherCommand(Guid Id) : ICommand;
