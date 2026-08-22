namespace Basket.Core.Entites;

public class ShoppingCart
{
    public string UserName {  get; set; }
    public List<ShoppingcartItem> Items { get; set; }=new List<ShoppingcartItem>();

    public ShoppingCart()
    {
        
    }
    public ShoppingCart(string userName)
    {
        UserName = userName;
    }
}
