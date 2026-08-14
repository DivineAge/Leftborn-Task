

using FluentValidation;

namespace Module.Playlist.Application.PlaylistSongs.RemoveSong;

internal sealed class RemoveSongCommandValidator : AbstractValidator<RemoveSongCommand>
{
    public RemoveSongCommandValidator()
    {
        RuleFor(x => x.PlaylistId).NotEmpty();
        RuleFor(x => x.SongId).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}
