using Basket.Core.Entites;

namespace Basket.Application.Response;

public class ShoppingCartItemResponseDto
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string ProductId { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public string ImageFile { get; set; } = null!;

}
