
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Test.Common.Presentation.Results;
using Test.Common.Presentation.Endpoints;
using Test.Common.Domain;
using Module.Users.Application.Users.RegisterUser;

namespace Module.Users.Presentation.Users;

internal sealed class RegisterUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (Request request, ISender sender) =>
         {
             Result<Guid> result = await sender.Send(new RegisterUserCommand(
                 request.FirstName,
                 request.LastName));

             return result.Match(Results.Ok, ApiResults.Problem);
         })
         .AllowAnonymous()
         .WithTags(Tags.Users);
    }
}

internal sealed class Request
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
}