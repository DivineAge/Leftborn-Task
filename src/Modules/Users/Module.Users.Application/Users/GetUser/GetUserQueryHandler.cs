using System.Data.Common;
using Dapper;
using Test.Common.Application.Messaging;
using Test.Common.Application.Data;
using Test.Common.Domain;
using Module.Users.Domain.Users;

namespace Module.Users.Application.Users.GetUser;

internal sealed class GetUserQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateDbConnectionAsync();
        const string sql =
                $"""
             SELECT
                 "Id" AS {nameof(UserResponse.Id)},
                 "FirstName" AS {nameof(UserResponse.FirstName)},
                 "LastName" AS {nameof(UserResponse.LastName)}
             FROM users."Users"
             WHERE "Id" = @UserId
             """;
        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, new { UserId = request.Id });
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserError.NotFound(request.Id));
        }
        return user;
    }
}