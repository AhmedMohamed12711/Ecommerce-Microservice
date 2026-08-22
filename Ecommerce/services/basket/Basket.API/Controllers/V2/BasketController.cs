using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Querise;
using Basket.Core.Entites;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2;

[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BasketController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;
    private readonly ILogger<BasketController> _logger;
    public BasketController(IMediator mediator
        , IPublishEndpoint publishEndpoint
        , IMapper mapper
        , ILogger<BasketController> logger)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _logger = logger;
    }


    [HttpPost("Checkout")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckout)
    {
        //get basket by username
        var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
        var basket = await _mediator.Send(query);
        if (basket == null)
            return BadRequest();
        var eventmsg = _mapper.Map<BasketCheckoutEventV2>(basketCheckout);
        eventmsg.TotalPrice = basket.TotalPrice;
        await _publishEndpoint.Publish(eventmsg);

        _logger.LogInformation($"Basket Published for {basket.UserName} with v2 endpoint");
        //remove from basket
        var deletecmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
        await _mediator.Send(deletecmd);
        return Accepted();
    }

}
