using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entites;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IMapper mapper, IProductRepository productRepository)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task<ProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var ProductEntity = _mapper.Map<Product>(request);
        var newProduct=await _productRepository.CreateProduct(ProductEntity);
        var productresponse = _mapper.Map<ProductResponseDto>(newProduct);
        return productresponse;
    }
}
