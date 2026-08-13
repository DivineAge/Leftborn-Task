
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Test.Common.Domain;
using Test.Common.Application.Messaging;

namespace Module.Songs.Application.Publisher.RegisterPublisher;

internal sealed class CreatePublisherCommandHandler(IPublisherRepository publisherRepository, IUnitOfWork unitOfWork) :
ICommandHandler<CreatePublisherCommand>
{
    public async Task<Result> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = Domain.Publisher.Publisher.Create(request.PublisherId, request.FirstName, request.LastName);

        publisherRepository.Insert(publisher);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();

    }
}
