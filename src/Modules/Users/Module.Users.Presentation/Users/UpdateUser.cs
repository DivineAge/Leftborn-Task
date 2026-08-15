
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Module.Users.Application.Users.UpdateUser;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Users.Presentation.Users;

internal sealed class UpdateUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id}", async ([FromRoute] Guid id, [FromBody] Request request, ISender sender) =>
        {
            Result result = await sender.Send(new UpdateUserCommand(id, request.FirstName, request.LastName));

            return result.Match(() => Results.Ok(), ApiResults.Problem);

        }).AllowAnonymous()
        .WithTags(Tags.Users);
    }
}
