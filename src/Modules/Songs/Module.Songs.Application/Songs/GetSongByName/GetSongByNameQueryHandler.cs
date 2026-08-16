

using System.Data.Common;
using Dapper;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Domain.Songs;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.GetSongByName;

internal sealed class GetSongByNameQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetSongByNameQuery, IEnumerable<SongResponse>>
{

    public async Task<Result<IEnumerable<SongResponse>>> Handle(GetSongByNameQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateDbConnectionAsync();
        const string sql =
            $"""
            SELECT
                "Id" AS {nameof(SongResponse.Id)},
                "Name" AS {nameof(SongResponse.Name)},
                "TimeInSeconds" AS {nameof(SongResponse.TimeInSeconds)},
                "PublisherId" AS {nameof(SongResponse.PublisherId)}
            FROM songs."Songs"
            WHERE "Name" = @Name
            """;

        IEnumerable<SongResponse> songs = await connection.QueryAsync<SongResponse>(sql, request);
        List<SongResponse> list = songs.ToList();

        if (list.Count == 0)
        {
            return Result.Failure<IEnumerable<SongResponse>>(SongErrors.NotFound(request.Name));
        }

        return list;
    }

}
