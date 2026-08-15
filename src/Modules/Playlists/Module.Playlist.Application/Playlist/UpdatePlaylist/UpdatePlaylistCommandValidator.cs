using FluentValidation;

namespace Module.Playlist.Application.Playlist.UpdatePlaylist;

internal sealed class UpdatePlaylistCommandValidator : AbstractValidator<UpdatePlaylistCommand>
{
    public UpdatePlaylistCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage("OwnerId is required.");
    }
}
