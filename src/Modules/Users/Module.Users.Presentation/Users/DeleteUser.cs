

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Module.Users.Application.Users.DeleteUser;
using Test.Common.Domain;
using Test.Common.Presentation.Endpoints;
using Test.Common.Presentation.Results;

namespace Module.Users.Presentation.Users;

internal sealed class DeleteUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/users/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
         {
             Result result = await sender.Send(new DeleteUserCommand(id), cancellationToken);

             return result.Match(() => Results.Ok(), ApiResults.Problem);


         })
         .AllowAnonymous()
         .WithTags("Users");
    }
}
