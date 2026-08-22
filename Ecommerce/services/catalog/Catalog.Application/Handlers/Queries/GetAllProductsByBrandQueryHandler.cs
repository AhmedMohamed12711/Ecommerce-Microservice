using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetAllProductsByBrandQueryHandler:IRequestHandler<GetAllProductsByBrandQuery,IList<ProductResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetAllProductsByBrandQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IList<ProductResponseDto>> Handle(GetAllProductsByBrandQuery request, CancellationToken cancellationToken)
    {
        var ProductName = await _productRepository.GetProductsByBrand(request.Brand);
        var ProductResponse = _mapper.Map<IList<ProductResponseDto>>(ProductName);
        return ProductResponse;
    }
}
