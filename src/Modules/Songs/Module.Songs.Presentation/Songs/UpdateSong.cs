
using MediatR;
using Microsoft.AspNetCore.Routing;
using Test.Common.Presentation.Endpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Module.Songs.Application.Songs.UpdateSong;
using Test.Common.Domain;
using static Module.Songs.Presentation.Songs.CreateSong;
using Test.Common.Presentation.Results;
using Microsoft.AspNetCore.Http;


namespace Module.Songs.Presentation.Songs;

internal sealed class UpdateSong() : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/songs/id/{id}", async ([FromRoute] Guid id,[FromBody] Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            Result result = await sender.Send(new UpdateSongCommand( id, request.PublisherId, request.Name, request.TimeInSeconds), cancellationToken);

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .WithName("UpdateSong")
        .WithTags("Songs");
    }
}
