using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Test.Common.Presentation.Results;
using Test.Common.Presentation.Endpoints;
using Test.Common.Domain;
using Module.Playlist.Application.PlaylistSongs.GetUserPlaylist;

namespace Module.Playlist.Presentation.Playlist.GetPlaylist;

internal sealed class GetPlaylist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/playlists/{playlistId}/users/{userId}", async (Guid userId, Guid playlistId, ISender sender) =>
        {
            Result<IEnumerable<UserPlaylistResponse>> result = await sender.Send(new GetUserPlaylistQuery(userId, playlistId));


            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags(Tags.Playlists);
    }
}
