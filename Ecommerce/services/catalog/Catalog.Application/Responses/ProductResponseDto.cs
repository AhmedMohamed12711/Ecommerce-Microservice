using Catalog.Core.Entites;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Responses;

public class ProductResponseDto
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageFile { get; set; } = null!;
    public string Summary { get; set; } = null!;
    [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
    public decimal Price { get; set; }

    public ProductBrand Brand { get; set; }
    public ProductType Type { get; set; }
}
