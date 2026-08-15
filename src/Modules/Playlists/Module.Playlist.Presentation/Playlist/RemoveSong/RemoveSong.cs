
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Playlist.Application.PlaylistSongs.RemoveSong;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

using Microsoft.AspNetCore.Mvc;

namespace Module.Playlist.Presentation.Playlist.RemoveSong;

internal sealed class RemoveSong : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/playlists/deletesong/", async ([FromBody] RemoveSongRequest request, ISender sender) =>
        {
            Result result = await sender.Send(new RemoveSongCommand(request.PlaylistId, request.SongId, request.OwnerId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags("Playlists");
    }
}

internal sealed class RemoveSongRequest
{
    public Guid PlaylistId { get; set; }
    public Guid SongId { get; set; }
    public Guid OwnerId { get; set; }
}
