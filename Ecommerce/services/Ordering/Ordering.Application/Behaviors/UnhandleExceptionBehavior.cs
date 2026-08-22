
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviors;

public class UnhandleExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest
{
    private readonly ILogger<TRequest> _logger;

    public UnhandleExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName=typeof(TRequest).Name;
            _logger.LogInformation($"Unhandled exception occured with requestname : {requestName} ,{request}");
            throw;
        }
    }
}
