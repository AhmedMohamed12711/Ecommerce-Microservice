using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Responses;

public class TypeResponseDto
{

    public string Id { get; set; }
    public string Name { get; set; } = null!;

}
