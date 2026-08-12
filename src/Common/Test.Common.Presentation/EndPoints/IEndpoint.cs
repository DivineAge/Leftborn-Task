using Microsoft.AspNetCore.Routing;

namespace Test.Common.Presentation.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
