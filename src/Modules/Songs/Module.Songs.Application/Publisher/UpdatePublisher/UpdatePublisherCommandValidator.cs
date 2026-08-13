

using FluentValidation;

namespace Module.Songs.Application.Publisher.UpdatePublisher;

internal sealed class UpdatePublisherCommandValidator : AbstractValidator<UpdatePublisherCommand>
{
    public UpdatePublisherCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty();
        RuleFor(p => p.PublisherId).NotEmpty();
        RuleFor(p => p.LastName).NotEmpty();

    }

}
