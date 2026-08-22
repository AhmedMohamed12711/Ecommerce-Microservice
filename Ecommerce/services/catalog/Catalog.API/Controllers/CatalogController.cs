using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Catalog.API.Controllers;


public class CatalogController : BaseApiController
{
     private readonly IMediator _mediator;
     private readonly ILogger<CatalogController> _logger;
    public CatalogController(IMediator mediator, ILogger<CatalogController> logger)
    {
        _mediator = mediator;   
        _logger = logger;
    }



    [HttpGet]
    [Route("GetProductById/{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType( (int)HttpStatusCode.NotFound)]

    public async Task<ActionResult<ProductResponseDto>> GetProductById(string id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        _logger.LogInformation("Product with id {ProductId} was fetched", id);
        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{productName}", Name = "GetProductByName")]
    [ProducesResponseType(typeof(IList<ProductResponseDto>), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ProductResponseDto>> GetAllProductByName(string productName)
    {
        var query = new GetAllProductsByNameQuery(productName);
        var result = await _mediator.Send(query);
        _logger.LogInformation($"product with {productName} is featched");
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAllProducts")]
    [ProducesResponseType(typeof(IList<ProductResponseDto>), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ProductResponseDto>> GetAllProducts([FromQuery]CatalogSpecParams specParams)
    {
        var query = new GetAllProductQuery(specParams);
        var result = await _mediator.Send(query);
        _logger.LogInformation("GetAllProducts was called with params: {@SpecParams}", specParams);
        return Ok(result);
    }


    [HttpGet]
    [Route("GetAllBrands")]
    [ProducesResponseType(typeof(IList<BrandResponseDto>), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<BrandResponseDto>> GetAllBrands()
    {
        var query = new GetAllBrandsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAllTypes")]
    [ProducesResponseType(typeof(IList<TypeResponseDto>), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<TypeResponseDto>> GetAllTypes()
    {
        var query = new GetAllTypeQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Route("CreateProduct")]
    [ProducesResponseType(typeof(ProductResponseDto), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] CreateProductCommand productCommand)
    {
        var result = await _mediator.Send<ProductResponseDto>(productCommand);
        return Ok(result);
    }


    [HttpPut]
    [Route("UpdateProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ProductResponseDto>> UpdateProduct([FromBody] UpdateProductCommand productCommand)
    {
        var result = await _mediator.Send<bool>(productCommand);
        return Ok(result);
    }



    [HttpDelete]
    [Route("{id}",Name ="DeleteProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]

    public async Task<ActionResult<ProductResponseDto>> DeleteProduct(string id)
    {
        var command=new DeleteProductCommand(id);
        var result = await _mediator.Send<bool>(command);
        return Ok(result);
    }
}
