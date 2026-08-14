
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Playlist.Application.PlaylistSongs.RemoveSong;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Playlist.Presentation.Playlist.RemoveSong;

internal sealed class RemoveSong : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/playlists/deletesong/{OwnerId}", async (Request request, Guid OwnerId, ISender sender) =>
        {
            Result result = await sender.Send(new RemoveSongCommand(request.PlaylistId, request.SongId, OwnerId));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags("Playlists");
    }
}

internal sealed class Request
{
    public Guid PlaylistId { get; set; }
    public Guid SongId { get; set; }
}
