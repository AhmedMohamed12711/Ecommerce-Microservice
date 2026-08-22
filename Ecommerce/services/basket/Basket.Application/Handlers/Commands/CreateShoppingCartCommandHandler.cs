using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Response;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.Commands;

public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponseDto>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IMapper _mapper;
    private readonly DiscountGrpcService _discountGrpcService;

    public CreateShoppingCartCommandHandler(IMapper mapper, IBasketRepository basketRepository, DiscountGrpcService discountGrpcService)
    {
        _mapper = mapper;
        _basketRepository = basketRepository;
        _discountGrpcService= discountGrpcService;
    }
    public async Task<ShoppingCartResponseDto> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
            if (coupon != null)
            {
                item.Price -= coupon.Amount;
            }

        }
        var shoppingCart = await _basketRepository.UpdateBasket(new Core.Entites.ShoppingCart()
        {
            Items = request.Items,
            UserName = request.UserName,
        });
        var shoppingCartResponse=_mapper.Map<ShoppingCartResponseDto>(shoppingCart);
        return shoppingCartResponse;
    }
}
