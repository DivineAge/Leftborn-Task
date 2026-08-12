using Test.Common.Domain;
using MediatR;

namespace Test.Common.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
