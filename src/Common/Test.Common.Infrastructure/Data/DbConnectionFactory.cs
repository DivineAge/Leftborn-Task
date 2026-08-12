using System.Data.Common;
using Test.Common.Application.Data;
using Npgsql;

namespace Test.Common.Infrastructure.Data;

internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{

    public async ValueTask<DbConnection> CreateDbConnectionAsync(CancellationToken cancellationToken = default)
    {
        return await dataSource.OpenConnectionAsync(cancellationToken);
    }
}

