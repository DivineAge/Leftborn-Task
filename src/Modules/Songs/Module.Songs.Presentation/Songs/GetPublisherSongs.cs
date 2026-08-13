

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Songs.Application.Songs.GetPublisherSongs;
using Module.Songs.Application.Songs.GetSong;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Songs.Presentation.Songs;

internal sealed class GetPublisherSongs : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("songs/publishers/{publisherId}", async (Guid publisherId, ISender sender) =>
        {
            Result<IEnumerable<SongResponse>> result = await sender.Send(new GetPublisherSongsQuery(publisherId));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Songs);
    }
}
