
using Microsoft.EntityFrameworkCore;
using Module.Users.Domain.Users;
using Module.Users.Infrastructure.Database;

namespace Module.Users.Infrastructure.Users;

internal sealed class UserRepository(UsersDbContext context) : IUserRepository
{
    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return context.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public void Insert(User user)
    {
        context.Users.Add(user);
    }

}
