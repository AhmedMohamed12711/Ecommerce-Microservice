using Ordering.Core.Entites;
namespace Ordering.Core.Repositories;
public interface IOrderRepository:IAsyncRepository<Order>
{
    Task<IEnumerable<Order>> GetOrderByUserName(string userName);
}
