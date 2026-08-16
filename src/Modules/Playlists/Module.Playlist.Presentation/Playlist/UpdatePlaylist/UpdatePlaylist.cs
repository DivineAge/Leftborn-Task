using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Test.Common.Presentation.Endpoints;
using MediatR;
using Module.Playlist.Application.Playlist.UpdatePlaylist;
using Test.Common.Presentation.Results;
using Microsoft.AspNetCore.Http;
using Test.Common.Domain;

namespace Module.Playlist.Presentation.Playlist.UpdatePlaylist;

internal sealed class UpdatePlaylist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/playlists/id/{id}", async ([FromRoute] Guid id, [FromBody] Request request, ISender sender) =>
        {
            Result result = await sender.Send(new UpdatePlaylistCommand(id, request.Name, request.OwnerId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        }).AllowAnonymous()
        .WithTags(Tags.Playlists);
    }
}
internal sealed record Request(string Name, Guid OwnerId);
