using AutoMapper;
using Basket.Application.Querise;
using Basket.Application.Response;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.Queries;

public class GetBasketByUserNameQueryHandler : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IBasketRepository _basketRepository;

    public GetBasketByUserNameQueryHandler(IMapper mapper, IBasketRepository basketRepository)
    {
        _mapper = mapper;
        _basketRepository=basketRepository;
    }
    public async Task<ShoppingCartResponseDto> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
    {
        var shoppingCart = await _basketRepository.GetBasket(request.UserName);
        var shoppingCartResponse=_mapper.Map<ShoppingCartResponseDto>(shoppingCart);
        return shoppingCartResponse;
    }
}
