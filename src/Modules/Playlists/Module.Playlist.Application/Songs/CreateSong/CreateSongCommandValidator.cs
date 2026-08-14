

using FluentValidation;

namespace Module.Playlist.Application.Songs.CreateSong;

internal sealed class CreateSongCommandValidator : AbstractValidator<CreateSongCommand>
{
    public CreateSongCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.TimeInSeconds)
            .GreaterThan(0).WithMessage("Time in seconds must be greater than 0.");

        RuleFor(x => x.PublisherId)
            .NotEmpty().WithMessage("PublisherId is required.");
    }
}
