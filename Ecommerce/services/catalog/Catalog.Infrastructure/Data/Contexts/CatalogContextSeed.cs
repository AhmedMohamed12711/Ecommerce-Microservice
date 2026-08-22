using Catalog.Core.Entites;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Contexts;

public class CatalogContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<Product> productcollection)
    {
        var FilePath = Path.Combine("Data", "SeedData", "products.json");
        if (!File.Exists(FilePath))
        {
            Console.WriteLine($"the file path not exists {FilePath}");
            return;
        }
        var productdata = await File.ReadAllTextAsync(FilePath);
        var products = JsonSerializer.Deserialize<List<Product>>(productdata);
        if (products?.Any() == true)
        {
            var countInDb = await productcollection.CountDocumentsAsync(_ => true);
            if (countInDb < products.Count)
            {
                await productcollection.DeleteManyAsync(_ => true);
                await productcollection.InsertManyAsync(products);
            }
        }
    }
}
