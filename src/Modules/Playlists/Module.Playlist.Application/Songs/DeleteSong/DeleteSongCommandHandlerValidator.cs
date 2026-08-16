using FluentValidation;
namespace Module.Playlist.Application.Songs.DeleteSong;

internal sealed class DeleteSongCommandHandlerValidator : AbstractValidator<DeleteSongCommand>
{
    public DeleteSongCommandHandlerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }

}
