using FluentValidation;

namespace Module.Songs.Application.Publisher.RegisterPublisher;

internal sealed class RegisterPublisherCommandValidator : AbstractValidator<CreatePublisherCommand>
{
    public RegisterPublisherCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.PublisherId).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();

    }
}
