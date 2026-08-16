using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Playlist.Application.Playlist.DeletePlaylist;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Playlist.Presentation.Playlist.DeletePlaylist;

internal sealed class DeletePlaylist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/playlists/{playlistId:guid}", async ([FromRoute] Guid playlistId, ISender sender) =>
        {
            Result result = await sender.Send(new DeletePlaylistCommand(playlistId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags(Tags.Playlists);
    }
}
