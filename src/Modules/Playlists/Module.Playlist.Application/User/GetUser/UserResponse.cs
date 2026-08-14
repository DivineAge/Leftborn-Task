

namespace Module.Playlist.Application.User.GetUser
;

internal sealed record UserResponse
(
    Guid UserId,
    string FirstName,
    string LastName
);
