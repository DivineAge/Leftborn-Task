using FluentValidation;
namespace Module.Playlist.Application.User.CreateUser;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(50).WithMessage("LastName must not exceed 50 characters.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }

}
