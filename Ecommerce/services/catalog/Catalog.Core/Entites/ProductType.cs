using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entites;

public class ProductType:BaseEntity
{
    //[BsonElement("name")]
    public string Name { get; set; } = null!;
}
