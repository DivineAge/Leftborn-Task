using Microsoft.EntityFrameworkCore;
using Module.Playlist.Domain.Users;
using Module.Playlist.Infrastructure.Database;

namespace Module.Playlist.Infrastructure.Users;

public class UserRepository(PlaylistDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public void Insert(User user)
    {
        dbContext.Users.Add(user);
    }
}
