using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetAllProductsByNameQueryHandler : IRequestHandler<GetAllProductsByNameQuery, IList<ProductResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetAllProductsByNameQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IList<ProductResponseDto>> Handle(GetAllProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var ProductName = await _productRepository.GetProductsByName(request.Name);
        var ProductResponse = _mapper.Map<IList<ProductResponseDto>>(ProductName.ToList());
        return ProductResponse;
    }
}
