

using Microsoft.Extensions.Logging;
using Ordering.Core.Entites;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext,ILogger<OrderContextSeed> logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.AddRange(GetOrders());
            await orderContext.SaveChangesAsync();
            logger.LogInformation($"Ordering Database : {typeof(OrderContext).Name} seeded!");
        }
    }

    public static IEnumerable<Order> GetOrders()
    {
        return new List<Order>
       {
           new Order
           {
               UserName="ahmed mohamed",
               FirstName="ahmed",
               LastName="mohamed",
               EmailAddress="ahmed@ecommerce.net",
               AddressLine="Cairo",
               Country="Egypt",
               TotalPrice=750,
               ZipCode="71111",
               CardName="Visa",
               CardNumber="1234567890123456",
               CreatedBy="ahmed",
               Expiration="12/26",
               Cvv="123",
               PaymentMethod=1,
               LastModifiedBy="ahmed",
               LastModifiedDate=new DateTime()

           }
       };
    }
}
