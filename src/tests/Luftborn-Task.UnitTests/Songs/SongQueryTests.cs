using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;
using Module.Songs.Application.Publisher.GetPublisher;
using Module.Songs.Application.Songs.GetAllSongs;
using Module.Songs.Application.Songs.GetPublisherSongs;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Application.Songs.GetSongByName;
using Module.Songs.Domain.Publisher;
using Module.Songs.Domain.Songs;
using Moq;
using Test.Common.Application.Data;
using Xunit;

namespace Luftborn_Task.UnitTests.Songs;

public class SongQueryTests
{
    static SongQueryTests()
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
    public async Task GetSong_ShouldReturnSong_WhenSongExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var song = Song.Create(publisherId, 180, "Hotel California");
        await using var dbConnection = await CreateInMemoryDbConnectionAsync(song, seedPublisher: null);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetSongQueryHandler(dbConnectionFactory.Object);

        // Act
        var result = await handler.Handle(new GetSongQuery(song.Id), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(song.Id, result.Value.Id);
        Assert.Equal("Hotel California", result.Value.Name);
        Assert.Equal(180, result.Value.TimeInSeconds);
        Assert.Equal(publisherId, result.Value.PublisherId);
    }

    [Fact]
    public async Task GetSong_ShouldReturnFailure_WhenSongDoesNotExist()
    {
        // Arrange
        await using var dbConnection = await CreateInMemoryDbConnectionAsync(seedSong: null, seedPublisher: null);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetSongQueryHandler(dbConnectionFactory.Object);

        // Act
        var result = await handler.Handle(new GetSongQuery(Guid.NewGuid()), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Song.NotFound", result.Error.Code);
    }

    [Fact]
    public async Task GetSongByName_ShouldReturnSong_WhenSongExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var song = Song.Create(publisherId, 210, "Imagine");
        await using var dbConnection = await CreateInMemoryDbConnectionAsync(song, seedPublisher: null);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetSongByNameQueryHandler(dbConnectionFactory.Object);

        // Act
        var result = await handler.Handle(new GetSongByNameQuery("Imagine"), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Imagine", result.Value.Name);
    }

    [Fact]
    public async Task GetAllSongs_ShouldReturnAllSongs()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var song = Song.Create(publisherId, 200, "Song 1");
        await using var dbConnection = await CreateInMemoryDbConnectionAsync(song, seedPublisher: null);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetAllSongsQueryHandler(dbConnectionFactory.Object);

        // Act
        var result = await handler.Handle(new GetAllSongsQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
    }

    [Fact]
    public async Task GetPublisherById_ShouldReturnPublisher_WhenPublisherExists()
    {
        // Arrange
        var publisherId = Guid.NewGuid();
        var publisher = Publisher.Create(publisherId, "John", "Lennon", "john@example.com");
        await using var dbConnection = await CreateInMemoryDbConnectionAsync(seedSong: null, publisher);

        var dbConnectionFactory = new Mock<IDbConnectionFactory>();
        dbConnectionFactory.Setup(x => x.CreateDbConnectionAsync())
            .ReturnsAsync(dbConnection);

        var handler = new GetPublisherByIdQueryHandler(dbConnectionFactory.Object);

        // Act
        var result = await handler.Handle(new GetPublisherQuery(publisherId), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(publisherId, result.Value.PublisherId);
        Assert.Equal("John", result.Value.FirstName);
        Assert.Equal("Lennon", result.Value.LastName);
    }

    private static async Task<DbConnection> CreateInMemoryDbConnectionAsync(Song? seedSong, Publisher? seedPublisher)
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        await using var setupCmd = connection.CreateCommand();
        setupCmd.CommandText = @"
            ATTACH DATABASE ':memory:' AS songs;
            CREATE TABLE songs.""Songs"" (
                ""Id"" GUID PRIMARY KEY,
                ""Name"" TEXT NOT NULL,
                ""TimeInSeconds"" INTEGER NOT NULL,
                ""PublisherId"" GUID NOT NULL
            );
            CREATE TABLE songs.""Publishers"" (
                ""Id"" GUID PRIMARY KEY,
                ""FirstName"" TEXT NOT NULL,
                ""LastName"" TEXT NOT NULL,
                ""Email"" TEXT NOT NULL
            );
        ";
        await setupCmd.ExecuteNonQueryAsync();

        if (seedSong is not null)
        {
            await using var insertSongCmd = connection.CreateCommand();
            insertSongCmd.CommandText = @"
                INSERT INTO songs.""Songs"" (""Id"", ""Name"", ""TimeInSeconds"", ""PublisherId"")
                VALUES (@Id, @Name, @TimeInSeconds, @PublisherId);
            ";
            insertSongCmd.Parameters.AddWithValue("@Id", seedSong.Id);
            insertSongCmd.Parameters.AddWithValue("@Name", seedSong.Name);
            insertSongCmd.Parameters.AddWithValue("@TimeInSeconds", seedSong.TimeInSeconds);
            insertSongCmd.Parameters.AddWithValue("@PublisherId", seedSong.PublisherId);
            await insertSongCmd.ExecuteNonQueryAsync();
        }

        if (seedPublisher is not null)
        {
            await using var insertPublisherCmd = connection.CreateCommand();
            insertPublisherCmd.CommandText = @"
                INSERT INTO songs.""Publishers"" (""Id"", ""FirstName"", ""LastName"", ""Email"")
                VALUES (@Id, @FirstName, @LastName, @Email);
            ";
            insertPublisherCmd.Parameters.AddWithValue("@Id", seedPublisher.Id);
            insertPublisherCmd.Parameters.AddWithValue("@FirstName", seedPublisher.FirstName);
            insertPublisherCmd.Parameters.AddWithValue("@LastName", seedPublisher.LastName);
            insertPublisherCmd.Parameters.AddWithValue("@Email", seedPublisher.Email);
            await insertPublisherCmd.ExecuteNonQueryAsync();
        }

        return connection;
    }
}
