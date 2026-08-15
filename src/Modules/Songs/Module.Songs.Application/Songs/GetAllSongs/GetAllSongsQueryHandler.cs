using System.Data.Common;
using Dapper;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Domain.Publisher;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.GetAllSongs;

internal sealed class GetAllSongsQueryHandler(IDbConnectionFactory dbConnectionFactory) : IQueryHandler<GetAllSongsQuery, IEnumerable<SongResponse>>
{
    public async Task<Result<IEnumerable<SongResponse>>> Handle(
        GetAllSongsQuery request,
         CancellationToken cancellationToken)
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
            """;
        IEnumerable<SongResponse> songs = await connection.QueryAsync<SongResponse>(sql);

        return songs.ToList();
    }

    
}
