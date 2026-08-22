
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.Data;

public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
{
    public OrderContext CreateDbContext(string[] args)
    {
        var optionBuilder=new DbContextOptionsBuilder<OrderContext>();
        optionBuilder.
            UseSqlServer("Server=orderdb;Database=OrderDb;User Id=sa;Password=P@ssw0rd123;TrustServerCertificate=true");
        return new OrderContext(optionBuilder.Options);
    }
}
