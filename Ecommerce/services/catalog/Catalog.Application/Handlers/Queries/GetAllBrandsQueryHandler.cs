using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entites;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IBrandRepository _brandRepository;

    public GetAllBrandsQueryHandler(IMapper mapper, IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
        _mapper = mapper; 
    }

    public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var BrandList=await _brandRepository.GetAllBrands();
        var BrandResponseList=_mapper.Map<IList<ProductBrand>,IList<BrandResponseDto>> (BrandList.ToList());
        return BrandResponseList;
    }
}
