

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
                "Id" AS {nameof(SongResponse.Id)},
                "Name" AS {nameof(SongResponse.Name)},
                "TimeInSeconds" AS {nameof(SongResponse.TimeInSeconds)},
                "PublisherId" AS {nameof(SongResponse.PublisherId)}
            FROM songs."Songs"
            WHERE "Id" = @songid
            """;

        SongResponse? song = await connection.QuerySingleOrDefaultAsync<SongResponse>(sql, new { songid = request.Id });

        if (song is null)
        {
            return Result.Failure<SongResponse>(SongErrors.NotFound(request.Id));
        }

        return song;
    }
}
