

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
                id AS {nameof(SongResponse.Id)},
                name AS {nameof(SongResponse.Name)},
                time_in_seconds AS {nameof(SongResponse.TimeInSeconds)},
                publisher_id AS {nameof(SongResponse.PublisherId)}
            FROM songs."Songs"
            WHERE publisher_id = @PublisherId
            """;
        IEnumerable<SongResponse> songs = await connection.QueryAsync<SongResponse>(sql, request.PublisherId);

        return songs.ToList();
    }
}
