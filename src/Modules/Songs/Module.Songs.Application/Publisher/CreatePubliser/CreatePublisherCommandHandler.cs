
using Module.Songs.Application.Abstractions.Data;
using Module.Songs.Domain.Publisher;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Publisher.CreatePubliser;

internal sealed class CreatePublisherCommandHandler(IPublisherRepository publisherRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreatePublisherCommand>
{
    public async Task<Result> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        Domain.Publisher.Publisher publisher = Domain.Publisher.Publisher.Create(request.PublisherId, request.FirstName, request.LastName);

        publisherRepository.Insert(publisher);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
