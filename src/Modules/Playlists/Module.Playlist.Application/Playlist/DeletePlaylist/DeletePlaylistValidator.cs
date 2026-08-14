using FluentValidation;

namespace Module.Playlist.Application.Playlist.DeletePlaylist;

internal sealed class DeletePlaylistValidator : AbstractValidator<DeletePlaylistCommand>
{
    public DeletePlaylistValidator()
    {
        RuleFor(x => x.PlaylistId).NotEmpty().WithMessage("PlaylistId is required.");
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage("OwnerId is required.");
    }
}
