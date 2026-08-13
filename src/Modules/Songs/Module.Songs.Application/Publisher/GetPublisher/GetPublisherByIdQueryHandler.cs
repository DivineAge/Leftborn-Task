
using System.Data.Common;
using Dapper;
using Module.Songs.Domain.Publisher;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Songs.Application.Publisher.GetPublisher;

internal sealed class GetPublisherByIdQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetPublisherQuery, PublisherResponse>
{
    public async Task<Result<PublisherResponse>> Handle(GetPublisherQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.CreateDbConnectionAsync();

        const string sql =
                $"""
             SELECT
                 "Id" AS {nameof(PublisherResponse.PublisherId)},
                 "FirstName" AS {nameof(PublisherResponse.FirstName)},
                 "LastName" AS {nameof(PublisherResponse.LastName)}
             FROM songs."Publishers"
             WHERE "Id" = @PublisherId
             """;
        PublisherResponse? publisher = await connection.QuerySingleOrDefaultAsync<PublisherResponse>(sql, new { request.PublisherId });
        if (publisher is null)
        {
            return Result.Failure<PublisherResponse>(PublisherError.NotFound(request.PublisherId));
        }

        return Result.Success(publisher);
    }

}
