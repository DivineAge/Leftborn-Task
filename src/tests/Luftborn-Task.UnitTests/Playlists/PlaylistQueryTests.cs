using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;
using Module.Playlist.Application.PlaylistSongs.GetUserPlaylist;
using Moq;
using Test.Common.Application.Data;
using Xunit;

namespace Luftborn_Task.UnitTests.Playlists;

public class PlaylistQueryTests
{
    static PlaylistQueryTests()
    {
        SqlMapper.AddTypeHandler(new GuidTypeHandler());
        SqlMapper.AddTypeHandler(new Int32TypeHandler());
    }

    private class Int32TypeHandler : SqlMapper.TypeHandler<int>
    {
        public override void SetValue(IDbDataParameter parameter, int value)
        {
            parameter.Value = value;
        }

        public override int Parse(object value)
        {
            return Convert.ToInt32(value);
        }
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
    public async Task GetUserPlaylist_ShouldReturnPlaylistSongs_WhenPlaylistAndOwnerMatch()
    {
        // Arrange
        var playlistId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var songId = Guid.NewGuid();
        var publisherId = Guid.NewGuid();

        await using var rawDbConnection = await CreateInMemoryDbConnectionAsync(playlistId, userId, songId, publisherId);
        var dbConnection = new PostgresToSqliteDbConnection(rawDbConnection);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetUserPlaylistQueryHandler(dbConnectionFactory.Object);

        // Act
        var result = await handler.Handle(new GetUserPlaylistQuery(userId, playlistId), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var songs = result.Value.ToList();
        Assert.Single(songs);
        Assert.Equal(songId, songs[0].SongId);
        Assert.Equal("Playlist Track", songs[0].Name);
        Assert.Equal(180, songs[0].TimeInSeconds);
        Assert.Equal(publisherId, songs[0].PublisherId);
    }

    private static async Task<DbConnection> CreateInMemoryDbConnectionAsync(Guid playlistId, Guid userId, Guid songId, Guid publisherId)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        await using var setupCmd = connection.CreateCommand();
        setupCmd.CommandText = @"
            ATTACH DATABASE ':memory:' AS playlists;
            CREATE TABLE playlists.""Playlists"" (
                ""Id"" GUID PRIMARY KEY,
                ""OwnerId"" GUID NOT NULL,
                ""Name"" TEXT NOT NULL
            );
            CREATE TABLE playlists.""Songs"" (
                ""Id"" GUID PRIMARY KEY,
                ""Name"" TEXT NOT NULL,
                ""TimeInSeconds"" INTEGER NOT NULL,
                ""PublisherId"" GUID NOT NULL
            );
            CREATE TABLE playlists.""PlaylistSongs"" (
                ""PlaylistId"" GUID NOT NULL,
                ""SongId"" GUID NOT NULL,
                PRIMARY KEY (""PlaylistId"", ""SongId"")
            );
        ";
        await setupCmd.ExecuteNonQueryAsync();

        await using var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = @"
            INSERT INTO playlists.""Playlists"" (""Id"", ""OwnerId"", ""Name"") VALUES (@PlaylistId, @UserId, 'My Playlist');
            INSERT INTO playlists.""Songs"" (""Id"", ""Name"", ""TimeInSeconds"", ""PublisherId"") VALUES (@SongId, 'Playlist Track', 180, @PublisherId);
            INSERT INTO playlists.""PlaylistSongs"" (""PlaylistId"", ""SongId"") VALUES (@PlaylistId, @SongId);
        ";
        insertCmd.Parameters.AddWithValue("@PlaylistId", playlistId);
        insertCmd.Parameters.AddWithValue("@UserId", userId);
        insertCmd.Parameters.AddWithValue("@SongId", songId);
        insertCmd.Parameters.AddWithValue("@PublisherId", publisherId);
        await insertCmd.ExecuteNonQueryAsync();

        return connection;
    }

    private class PostgresToSqliteDbConnection : DbConnection
    {
        private readonly DbConnection _inner;
        public PostgresToSqliteDbConnection(DbConnection inner) => _inner = inner;

        public override string ConnectionString { get => _inner.ConnectionString; set => _inner.ConnectionString = value; }
        public override string Database => _inner.Database;
        public override string DataSource => _inner.DataSource;
        public override string ServerVersion => _inner.ServerVersion;
        public override ConnectionState State => _inner.State;

        public override void ChangeDatabase(string databaseName) => _inner.ChangeDatabase(databaseName);
        public override void Close() => _inner.Close();
        public override void Open() => _inner.Open();
        public override Task OpenAsync(CancellationToken cancellationToken) => _inner.OpenAsync(cancellationToken);

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => _inner.BeginTransaction(isolationLevel);
        protected override DbCommand CreateDbCommand() => new PostgresToSqliteDbCommand(_inner.CreateCommand());

        protected override void Dispose(bool disposing)
        {
            if (disposing) _inner.Dispose();
            base.Dispose(disposing);
        }

        public override ValueTask DisposeAsync() => _inner.DisposeAsync();
    }

    private class PostgresToSqliteDbCommand : DbCommand
    {
        private readonly DbCommand _inner;
        public PostgresToSqliteDbCommand(DbCommand inner) => _inner = inner;

        public override string CommandText
        {
            get => _inner.CommandText;
            set => _inner.CommandText = value?.Replace("::uuid", "");
        }
        public override int CommandTimeout { get => _inner.CommandTimeout; set => _inner.CommandTimeout = value; }
        public override CommandType CommandType { get => _inner.CommandType; set => _inner.CommandType = value; }
        public override bool DesignTimeVisible { get => _inner.DesignTimeVisible; set => _inner.DesignTimeVisible = value; }
        public override UpdateRowSource UpdatedRowSource { get => _inner.UpdatedRowSource; set => _inner.UpdatedRowSource = value; }
        protected override DbConnection? DbConnection { get => _inner.Connection; set => _inner.Connection = value; }
        protected override DbParameterCollection DbParameterCollection => _inner.Parameters;
        protected override DbTransaction? DbTransaction { get => _inner.Transaction; set => _inner.Transaction = value; }

        public override void Cancel() => _inner.Cancel();
        public override int ExecuteNonQuery() => _inner.ExecuteNonQuery();
        public override object? ExecuteScalar() => _inner.ExecuteScalar();
        public override void Prepare() => _inner.Prepare();
        protected override DbParameter CreateDbParameter() => _inner.CreateParameter();
        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) => _inner.ExecuteReader(behavior);
        protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        {
            return _inner.ExecuteReaderAsync(behavior, cancellationToken);
        }
    }
}
