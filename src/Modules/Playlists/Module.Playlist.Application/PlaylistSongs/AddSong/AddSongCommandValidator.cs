using FluentValidation;

namespace Module.Playlist.Application.PlaylistSongs.AddSong;

internal sealed class AddSongCommandValidator : AbstractValidator<AddSongCommand>
{
    public AddSongCommandValidator()
    {
        RuleFor(x => x.PlaylistId).NotEmpty();
        RuleFor(x => x.SongId).NotEmpty();
        RuleFor(x => x.OwnerId).NotEmpty();
    }
}
