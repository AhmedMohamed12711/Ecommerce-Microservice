using Basket.Core.Entites;

namespace Basket.Application.Response;

public class ShoppingCartResponseDto
{
    public string UserName { get; set; }=string.Empty;
    public List<ShoppingcartItem> Items { get; set; } = new List<ShoppingcartItem>();

    public ShoppingCartResponseDto()
    {

    }
    public ShoppingCartResponseDto(string userName)
    {
        UserName = userName;
    }
    public decimal TotalPrice
    {
        get
        {
            decimal totalPrice = 0;
            foreach (ShoppingcartItem item in Items)
            {
                totalPrice += item.Price * item.Quantity;
            }
            return totalPrice;
        }
    }
}

