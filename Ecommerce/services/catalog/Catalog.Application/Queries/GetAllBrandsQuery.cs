using Catalog.Application.Responses;
using Catalog.Core.Entites;
using MediatR;

namespace Catalog.Application.Queries;

public class GetAllBrandsQuery:IRequest<IList<BrandResponseDto>>
{

}
