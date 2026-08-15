

using FluentValidation;

namespace Module.Playlist.Application.User.UpdateUser;

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty();
        RuleFor(p => p.UserId).NotEmpty();
        RuleFor(p => p.LastName).NotEmpty();

    }

}
