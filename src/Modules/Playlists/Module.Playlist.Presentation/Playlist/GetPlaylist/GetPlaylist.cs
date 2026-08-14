
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
        app.MapGet("/api/playlists/", async (Request request, ISender sender) =>
        {
            Result<IEnumerable<UserPlaylistResponse>> result = await sender.Send(new GetUserPlaylistQuery(request.UserId, request.PlaylistId));


            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags("Playlists");
    }
}
internal sealed class Request
{
    public Guid PlaylistId { get; init; }
    public Guid UserId { get; init; }
}
