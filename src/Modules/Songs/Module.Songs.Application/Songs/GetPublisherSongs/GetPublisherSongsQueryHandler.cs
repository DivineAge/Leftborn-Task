

using System.Data.Common;
using Dapper;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Domain.Publisher;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Songs.GetPublisherSongs;

internal sealed class GetPublisherSongsQueryHandler(
    IDbConnectionFactory dbConnectionFactory,
    IPublisherRepository publisherRepository) :
    IQueryHandler<GetPublisherSongsQuery, IEnumerable<SongResponse>>
{
    public async Task<Result<IEnumerable<SongResponse>>> Handle(GetPublisherSongsQuery request, CancellationToken cancellationToken)
    {
        Domain.Publisher.Publisher? publihser = await publisherRepository.GetAsync(request.PublisherId, cancellationToken);
        if (publihser is null)
        {
            return Result.Failure<IEnumerable<SongResponse>>(PublisherError.NotFound(request.PublisherId));
        }
        await using DbConnection connection = await dbConnectionFactory.CreateDbConnectionAsync();

        const string sql =
            $"""
            SELECT
                "Id" AS {nameof(SongResponse.Id)},
                "Name" AS {nameof(SongResponse.Name)},
                "TimeInSeconds" AS {nameof(SongResponse.TimeInSeconds)},
                "PublisherId" AS {nameof(SongResponse.PublisherId)}
            FROM songs."Songs"
            WHERE "PublisherId" = @publisherid::uuid
            """;
        IEnumerable<SongResponse> songs = await connection.QueryAsync<SongResponse>(sql, new { publisherid = request.PublisherId });

        return songs.ToList();
    }
}
