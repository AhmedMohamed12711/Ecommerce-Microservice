using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entites;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetAllProductQueryhandler : IRequestHandler<GetAllProductQuery, Pagination<ProductResponseDto> >
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetAllProductQueryhandler(IMapper mapper, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Pagination<ProductResponseDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        var ProductList = await _productRepository.GetAllProducts(request.SpecParams);
        var ProductResponseList = _mapper.Map<Pagination<ProductResponseDto>>(ProductList);
        return ProductResponseList;
    }
}
