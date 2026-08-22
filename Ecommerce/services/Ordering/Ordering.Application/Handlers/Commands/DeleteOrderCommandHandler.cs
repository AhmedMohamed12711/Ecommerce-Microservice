
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptionsp;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;
    public DeleteOrderCommandHandler(IOrderRepository orderRepository,ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var orderDeleted = await _orderRepository.GetByIdAsync(request.Id);
        if(orderDeleted == null)
        {
            throw new OrderNotFoundException(nameof(Order), request.Id);
        }
       await _orderRepository.DeleteAsync(orderDeleted);
        _logger.LogInformation($"Order with id : {orderDeleted.Id} was deleted succssfully");
        return Unit.Value;
    }
}
