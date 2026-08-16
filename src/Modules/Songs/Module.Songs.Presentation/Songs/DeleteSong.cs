using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Songs.Application.Songs.DeleteSong;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Songs.Presentation.Songs;

internal sealed class DeleteSong() : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/songs/{songId:guid}", async ([FromRoute] Guid songId, ISender sender) =>
        {
            Result result = await sender.Send(new DeleteSongCommand(songId));

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .AllowAnonymous()
        .WithTags("Songs");
    }
}
