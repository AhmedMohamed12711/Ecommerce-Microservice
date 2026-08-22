using Basket.Application.Response;
using Basket.Core.Entites;
using MediatR;

namespace Basket.Application.Commands;

public class CreateShoppingCartCommand:IRequest<ShoppingCartResponseDto>
{
    public string UserName { get; set; } = string.Empty;
    public List<ShoppingcartItem> Items { get; set; } 

    public CreateShoppingCartCommand(string userName, List<ShoppingcartItem> items)
    {
        UserName = userName;
        Items = items;

    }
}
