using AutoMapper;
using Basket.Application.Response;
using Basket.Core.Entites;
using EventBus.Messages.Events;

namespace Basket.Application.Mapper;

public class BasketMappingProfile:Profile
{
    public BasketMappingProfile()
    {
        CreateMap<ShoppingCart,ShoppingCartResponseDto>().ReverseMap();
        CreateMap<ShoppingcartItem,ShoppingCartItemResponseDto>().ReverseMap();
        CreateMap<BasketCheckout,BasketCheckoutEvent>().ReverseMap();
        CreateMap<BasketCheckoutV2,BasketCheckoutEventV2>().ReverseMap();
    }
}
