using Catalog.Application.Responses;
using Catalog.Core.Entites;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllProductsByBrandQuery:IRequest<IList<ProductResponseDto>>
{
    public string Brand { get; set; }

    public GetAllProductsByBrandQuery(string brand)
    {
        Brand = brand;
    }

}
