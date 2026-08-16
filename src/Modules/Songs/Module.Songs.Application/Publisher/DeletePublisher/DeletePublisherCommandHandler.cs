
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Publisher.DeletePublisher;

internal sealed class DeletePublisherCommandHandler(IPublisherRepository publisherRepository, IUnitOfWork unitOfWork) : ICommandHandler<DeletePublisherCommand>
{
    public async Task<Result> Handle(DeletePublisherCommand command, CancellationToken cancellationToken)
    {
        Domain.Publisher.Publisher? publisher = await publisherRepository.GetAsync(command.Id, cancellationToken);

        if (publisher is null)
        {
            return Result.Failure(PublisherErrors.NotFound(command.Id));
        }

        publisherRepository.Delete(publisher);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

