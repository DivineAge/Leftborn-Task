using System.Data.Common;
using Dapper;
using Test.Common.Application.Messaging;
using Test.Common.Application.Data;
using Test.Common.Domain;
using Modules.Users.Domain.Users;

namespace Modules.Users.Application.Users.GetUser;

internal sealed class GetUserQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateDbConnectionAsync(cancellationToken);
        const string sql =
                $"""
             SELECT
                 id AS {nameof(UserResponse.Id)},
                 first_name AS {nameof(UserResponse.FirstName)},
                 last_name AS {nameof(UserResponse.LastName)}
             FROM users.users
             WHERE id = @UserId
             """;
        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, new { UserId = request.Id });
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserError.NotFound(request.Id));
        }
        return user;
    }
}