using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetAllProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public GetAllProductByIdQueryHandler(IMapper mapper, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var ProductById=await _productRepository.GetProductById(request.Id);
        var ProductResponse=_mapper.Map<ProductResponseDto>(ProductById);
        return ProductResponse;
    }

}
