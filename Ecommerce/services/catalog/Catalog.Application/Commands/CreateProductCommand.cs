using Catalog.Application.Responses;
using Catalog.Core.Entites;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Commands;

public class CreateProductCommand:IRequest<ProductResponseDto>
{
 
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageFile { get; set; } = null!;
    public string Summary { get; set; } = null!;
    [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
    public decimal Price { get; set; }

    public ProductBrand Brand { get; set; }
    public ProductType Type { get; set; }
}
