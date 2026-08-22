

using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands;

public class CheckoutOrderCommandV2handler: IRequestHandler<CheckoutOrderCommandV2, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CheckoutOrderCommandV2handler> _logger;
    public CheckoutOrderCommandV2handler(IOrderRepository orderRepository
        , IMapper mapper
        , ILogger<CheckoutOrderCommandV2handler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<int> Handle(CheckoutOrderCommandV2 request, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<Order>(request);
        var generatedOrder = await _orderRepository.AddAsync(orderEntity);
        _logger.LogInformation($"Order with id {generatedOrder.Id} successfully created with v2 handler");
        return generatedOrder.Id;
    }
}
