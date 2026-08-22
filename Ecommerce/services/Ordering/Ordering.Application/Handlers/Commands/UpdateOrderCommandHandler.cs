
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptionsp;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;
    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderUpdated = await _orderRepository.GetByIdAsync(request.Id);
        if (orderUpdated == null)
        {
            throw new OrderNotFoundException(nameof(Order), request.Id);
        }
        await _orderRepository.UpdateAsync(orderUpdated);
        _logger.LogInformation($"Order with id : {orderUpdated.Id} was updated succssfully");
        return Unit.Value;
    }
}
