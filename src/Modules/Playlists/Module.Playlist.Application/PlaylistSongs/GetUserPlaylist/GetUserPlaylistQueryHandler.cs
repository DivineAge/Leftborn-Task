
using System.Data.Common;
using Dapper;
using Module.Playlist.Domain.Songs;
using Test.Common.Application.Data;
using Test.Common.Application.Messaging;
using Test.Common.Domain;

namespace Module.Playlist.Application.PlaylistSongs.GetUserPlaylist
{
    internal sealed class GetUserPlaylistQueryHandler(
        IDbConnectionFactory dbConnectionFactory
    )
    : IQueryHandler<GetUserPlaylistQuery, IEnumerable<UserPlaylistResponse>>
    {

        public async Task<Result<IEnumerable<UserPlaylistResponse>>> Handle(GetUserPlaylistQuery query, CancellationToken cancellationToken)
        {
            await using DbConnection dbConnection = await dbConnectionFactory.CreateDbConnectionAsync();
            const string sql = $"""
                SELECT
                    s."Id"            AS {nameof(UserPlaylistResponse.SongId)},
                    s."Name"          AS {nameof(UserPlaylistResponse.Name)},
                    s."TimeInSeconds" AS {nameof(UserPlaylistResponse.TimeInSeconds)},
                    s."PublisherId"   AS {nameof(UserPlaylistResponse.PublisherId)}

                FROM playlists."Songs" AS s

                JOIN playlists."PlaylistSongs" AS ps
                    ON s."Id" = ps."SongId"

                JOIN playlists."Playlists" AS p
                    ON ps."PlaylistId" = p."Id"

                WHERE
                    p."Id" = @playlistId::uuid
                    AND p."OwnerId" = @userId::uuid
                """;
            IEnumerable<UserPlaylistResponse> songs =
             await dbConnection.QueryAsync<UserPlaylistResponse>(sql, new { playlistId = query.PlaylistId, userId = query.UserId });

            return songs.ToList();
        }

    }
}