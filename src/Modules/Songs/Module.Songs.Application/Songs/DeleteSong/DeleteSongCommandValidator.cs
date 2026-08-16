

using FluentValidation;

namespace Module.Songs.Application.Songs.DeleteSong;

internal sealed class DeleteSongCommandValidator : AbstractValidator<DeleteSongCommand>
{
    public DeleteSongCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }

}
