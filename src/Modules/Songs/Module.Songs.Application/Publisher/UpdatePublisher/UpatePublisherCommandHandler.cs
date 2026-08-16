

using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Test.Common.Domain;
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Publisher.UpdatePublisher;

internal sealed class UpatePublisherCommandHandler(IPublisherRepository publisherRepository, IUnitOfWork unitOfWork) : ICommandHandler<UpdatePublisherCommand>
{
    public async Task<Result> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        Domain.Publisher.Publisher? publisher = await publisherRepository.GetAsync(request.PublisherId, cancellationToken);

        if (publisher is null)
        {
            return Result.Failure(PublisherErrors.NotFound(request.PublisherId));
        }

        publisher.Update(request.FirstName, request.LastName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

}
