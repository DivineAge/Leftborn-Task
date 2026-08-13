
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Songs.Application.Songs.CreateSong;
using Module.Songs.Application.Songs.GetSong;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Songs.Presentation.Songs;

internal sealed class CreateSong : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("songs", async (CreateSongRequest request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new CreateSongCommand(request.Name, request.TimeInSeconds, request.PublisherId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Songs);
    }
    internal sealed class CreateSongRequest()
    {
        public Guid PublisherId { get; init; }
        public int TimeInSeconds { get; init; }
        public string Name { get; init; }
    }
}
