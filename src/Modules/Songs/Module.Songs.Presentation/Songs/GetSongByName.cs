using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Songs.Application.Songs.GetSong;
using Module.Songs.Application.Songs.GetSongByName;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Songs.Presentation.Songs;

internal sealed class GetSongByName : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("songs/{name}", async (string name, ISender sender) =>
         {
             Result<SongResponse> result = await sender.Send(new GetSongByNameQuery(name));

             return result.Match(Results.Ok, ApiResults.Problem);
         })
         .WithTags(Tags.Songs);
    }
}
