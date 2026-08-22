using Catalog.Core.Entites;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Contexts;

public static class BrandContextSeed
{
    public static async Task SeedDataAsync(IMongoCollection<ProductBrand> brandcollection) 
    { 
        var hasbrand=await brandcollection.Find(_=>true).AnyAsync();
        if(hasbrand)
            return;
        var FilePath = Path.Combine("Data", "SeedData", "brands.json");
        if (!File.Exists(FilePath))
        {
            Console.WriteLine($"the file path not exists{FilePath}");
            return;
        }
        var branddata=await File.ReadAllTextAsync(FilePath);
        var brands=JsonSerializer.Deserialize<List<ProductBrand>>(branddata);
        if (brands?.Any()==true)
        { 
            await brandcollection.InsertManyAsync(brands);
        }

    
    }

}
