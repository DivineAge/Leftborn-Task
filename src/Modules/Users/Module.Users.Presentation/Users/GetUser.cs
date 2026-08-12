
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Test.Common.Presentation.Results;
using Test.Common.Presentation.Endpoints;
using Test.Common.Domain;
using Module.Users.Application.Users.GetUser;
namespace Module.Users.Presentation.Users;

internal sealed class GetUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/profile", async (ISender sender, Guid id) =>
        {
            Result<UserResponse> result = await sender.Send(new GetUserQuery(id));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Users);
    }
}
