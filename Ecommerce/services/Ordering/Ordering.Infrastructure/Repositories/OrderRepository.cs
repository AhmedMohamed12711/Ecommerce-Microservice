using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entites;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext dbContext) : base(dbContext) { }

    public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
    {
       var OrderList= await _orderContext.Orders.Where(o=>o.UserName == userName).ToListAsync();
        return OrderList;
    }

}
