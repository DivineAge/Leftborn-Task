using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Test.Common.Presentation.Results;
using Test.Common.Presentation.Endpoints;
using Test.Common.Domain;
using Module.Playlist.Application.Playlist.CreatePlaylist;

using Microsoft.AspNetCore.Mvc;


namespace Module.Playlist.Presentation.Playlist.CreatePlaylist;

internal sealed class CreatePlaylist : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/playlist", async ([FromBody]CreatePlaylistRequest request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new CreatePlaylistCommand(request.UserId, request.Name));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags("Playlists");
    }
}

internal sealed class CreatePlaylistRequest
{
    public Guid UserId { get; init; }
    public string Name { get; init; }
}
