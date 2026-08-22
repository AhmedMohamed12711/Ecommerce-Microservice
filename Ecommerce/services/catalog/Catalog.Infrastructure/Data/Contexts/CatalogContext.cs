using Catalog.Core.Entites;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Contexts;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }

    public IMongoCollection<ProductBrand>Brands { get; }

    public IMongoCollection<ProductType> Types { get; }

    public CatalogContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration["DatabaseSetting:ConnectionString"]);//بينشئ Connection مع MongoDB.
        var database = client.GetDatabase(configuration["DatabaseSetting:DatabaseName"]);//بعد ما اتصلنا بالسيرفر بنختار قاعدة البيانات.

        Brands = database.GetCollection<ProductBrand>(configuration["DatabaseSetting:BrandsCollection"]);
        Types = database.GetCollection<ProductType>(configuration["DatabaseSetting:TypesCollection"]);
        Products = database.GetCollection<Product>(configuration["DatabaseSetting:ProductsCollection"]);



        BrandContextSeed.SeedDataAsync(Brands).GetAwaiter().GetResult();
        CatalogContextSeed.SeedDataAsync(Products).GetAwaiter().GetResult();
        TypeContextSeed.SeedDataAsync(Types).GetAwaiter().GetResult();
    }


}
