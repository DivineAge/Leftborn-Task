

using System.Data.Common;
using Dapper;
using Module.Songs.Domain.Songs;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.GetSong;

internal sealed class GetSongQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetSongQuery, SongResponse>
{
    public async Task<Result<SongResponse>> Handle(GetSongQuery request, CancellationToken cancellationToken)
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
            WHERE id = @SongId
            """;

        SongResponse? song = await connection.QuerySingleOrDefaultAsync<SongResponse>(sql, request);

        if (song is null)
        {
            return Result.Failure<SongResponse>(SongError.NotFound(request.Id));
        }

        return song;
    }
}
