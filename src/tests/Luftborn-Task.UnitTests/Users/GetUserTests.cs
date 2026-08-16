using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;
using Module.Users.Application.Users.GetUser;
using Module.Users.Domain.Users;
using Moq;
using Test.Common.Application.Data;
using Xunit;

namespace Luftborn_Task.UnitTests.Users;

public class GetUserTests
{
    static GetUserTests()
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
    }

    private class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }

        public override Guid Parse(object value)
        {
            return value switch
            {
                Guid guid => guid,
                string str => Guid.Parse(str),
                byte[] bytes => new Guid(bytes),
                _ => throw new InvalidCastException($"Cannot convert {value?.GetType()} to Guid")
            };
        }
    }

    [Fact]
    public async Task GetUser_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var seededUser = User.Create("Query", "User", "query@example.com");
        await using var dbConnection = await CreateInMemoryDbConnectionAsync(seededUser);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetUserQueryHandler(dbConnectionFactory.Object);

        // Act
        var result = await handler.Handle(new GetUserQuery(seededUser.Id), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(seededUser.Id, result.Value.Id);
        Assert.Equal("Query", result.Value.FirstName);
        Assert.Equal("User", result.Value.LastName);
        Assert.Equal("query@example.com", result.Value.Email);
    }

    [Fact]
    public async Task GetUser_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        // Arrange
        await using var dbConnection = await CreateInMemoryDbConnectionAsync(seedUser: null);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetUserQueryHandler(dbConnectionFactory.Object);
        var searchId = Guid.NewGuid();

        // Act
        var result = await handler.Handle(new GetUserQuery(searchId), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Users.NotFound", result.Error.Code);
    }

    private static async Task<DbConnection> CreateInMemoryDbConnectionAsync(User? seedUser)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        await using var setupCmd = connection.CreateCommand();
        setupCmd.CommandText = @"
            ATTACH DATABASE ':memory:' AS users;
            CREATE TABLE users.""Users"" (
                ""Id"" GUID PRIMARY KEY,
                ""FirstName"" TEXT NOT NULL,
                ""LastName"" TEXT NOT NULL,
                ""Email"" TEXT NOT NULL
            );
        ";
        await setupCmd.ExecuteNonQueryAsync();

        if (seedUser is not null)
        {
            await using var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO users.""Users"" (""Id"", ""FirstName"", ""LastName"", ""Email"")
                VALUES (@Id, @FirstName, @LastName, @Email);
            ";
            insertCmd.Parameters.AddWithValue("@Id", seedUser.Id);
            insertCmd.Parameters.AddWithValue("@FirstName", seedUser.FirstName);
            insertCmd.Parameters.AddWithValue("@LastName", seedUser.LastName);
            insertCmd.Parameters.AddWithValue("@Email", seedUser.Email);
            await insertCmd.ExecuteNonQueryAsync();
        }

        return connection;
    }
}
