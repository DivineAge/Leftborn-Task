
using System.Data.Common;
using Dapper;
using Module.Playlist.Domain.Users;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.User.GetUser;

internal sealed class GetUserByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateDbConnectionAsync();

        const string sql =
                $"""
             SELECT
                 "Id" AS {nameof(UserResponse.UserId)},
                 "FirstName" AS {nameof(UserResponse.FirstName)},
                 "LastName" AS {nameof(UserResponse.LastName)}
             FROM playlist."Users"
             WHERE "Id" = @UserId
             """;
        UserResponse? user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, new { request.UserId });
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserError.NotFound(request.UserId));
        }

        return Result.Success(user);
    }

}
