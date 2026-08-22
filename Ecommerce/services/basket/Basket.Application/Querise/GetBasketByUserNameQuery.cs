using Basket.Application.Response;
using MediatR;

namespace Basket.Application.Querise;

public class GetBasketByUserNameQuery:IRequest<ShoppingCartResponseDto>
{
    public string UserName { get; set; }
    public GetBasketByUserNameQuery(string userName)
    {
        UserName = userName;
    }
}
