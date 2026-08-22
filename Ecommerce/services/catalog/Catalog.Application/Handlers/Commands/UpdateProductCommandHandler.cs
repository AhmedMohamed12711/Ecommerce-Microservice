using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entites;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Commands;

public class UpdateProductCommandHandler:IRequestHandler<UpdateProductCommand,bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler( IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var ProductEntity = await _productRepository.UpdateProduct(new Core.Entites.Product
        {
            Brand = request.Brand,
            Description = request.Description,  
            Id = request.Id,
            ImageFile = request.ImageFile,
            Name = request.Name,
            Price = request.Price,
            Summary = request.Summary,
            Type=request.Type
        });
        return ProductEntity;
    }
}
