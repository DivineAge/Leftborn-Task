
using System.Data.Common;

namespace Test.Common.Application.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> CreateDbConnectionAsync();
}

