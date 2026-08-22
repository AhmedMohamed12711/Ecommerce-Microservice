
using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context=new ValidationContext<TRequest>(request);
            //will run all validator rules one by on eand return validation result
            var validationResult = await Task.WhenAll(
                _validators.Select(v=>v.ValidateAsync(context,cancellationToken)));

            //now need check all failure
            var failurs = validationResult.SelectMany(e => e.Errors).Where(f => f != null).ToList();
            if (failurs.Count!=0)
            { 
              throw new ValidationException(failurs);
            }
        }
            return await next();
    }
}
