using Catalog.Core.Entites;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Contexts;

public static class TypeContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<ProductType> typecollection)
    {
        var hastypes = await typecollection.Find(_ => true).AnyAsync();
        if (hastypes)
            return;
        var FilePath = Path.Combine("Data", "SeedData", "types.json");
        if (!File.Exists(FilePath))
        {
            Console.WriteLine($"the file path not exists{FilePath}");
            return;
        }
        var typedata = await File.ReadAllTextAsync(FilePath);
        var types = JsonSerializer.Deserialize<List<ProductType>>(typedata);
        if (types?.Any() == true)
        {
            await typecollection.InsertManyAsync(types);
        }


    }
}
