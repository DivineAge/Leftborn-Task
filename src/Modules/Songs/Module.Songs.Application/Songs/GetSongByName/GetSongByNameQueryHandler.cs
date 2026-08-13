

using System.Data.Common;
using Dapper;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Domain.Songs;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.GetSongByName;

internal sealed class GetSongByNameQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetSongByNameQuery, SongResponse>
{

    public async Task<Result<SongResponse>> Handle(GetSongByNameQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateDbConnectionAsync();
        const string sql =
            $"""
            SELECT
                id AS {nameof(SongResponse.Id)},
                name AS {nameof(SongResponse.Name)},
                time_in_seconds AS {nameof(SongResponse.TimeInSeconds)},
                publisher_id AS {nameof(SongResponse.PublisherId)}
            FROM songs."Songs"
            WHERE name = @Name
            """;

        SongResponse? song = await connection.QuerySingleOrDefaultAsync<SongResponse>(sql, request);

        if (song is null)
        {
            return Result.Failure<SongResponse>(SongError.NotFound(request.Name));
        }

        return Result.Success(song);
    }

}
