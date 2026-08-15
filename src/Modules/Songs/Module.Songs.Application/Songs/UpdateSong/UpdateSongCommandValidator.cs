using FluentValidation;

namespace Module.Songs.Application.Songs.UpdateSong;

internal sealed  class UpdateSongCommandValidator : AbstractValidator<UpdateSongCommand>
{
    public UpdateSongCommandValidator()
    {
        RuleFor(x => x.SongId).NotEmpty().WithMessage("SongId is required.");
        RuleFor(x => x.PublisherId).NotEmpty().WithMessage("PublisherId is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.TimeInSeconds).GreaterThan(0).WithMessage("TimeInSeconds must be greater than 0.");
    }
}
        
    
