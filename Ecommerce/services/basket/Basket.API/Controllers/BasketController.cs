using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.GrpcServices;
using Basket.Application.Querise;
using Basket.Application.Response;
using Basket.Core.Entites;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiVersion("1")]
    public class BasketController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketController> _logger;
        public BasketController(IMediator mediator
            , IPublishEndpoint publishEndpoint
            , IMapper mapper
            ,ILogger<BasketController> logger)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        [Route("[action]/{userName}",Name ="GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponseDto),200)]
        public async Task<ActionResult<ShoppingCartResponseDto>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponseDto), 200)]

        public async Task<ActionResult<ShoppingCartResponseDto>> UpdateBasket([FromBody]CreateShoppingCartCommand command) 
        {
           
            var basket=await _mediator.Send(command);
            return Ok(basket);
        }

        [HttpDelete("DeleteBasket/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponseDto>> DeleteBasket(string userName)
        {
            var command = new DeleteBasketByUserNameCommand(userName);
            var basket = await _mediator.Send(command);
            return Ok(basket);
        }


        [HttpPost("Checkout")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            //get basket by username
            var query=new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
                return BadRequest();
            var eventmsg = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventmsg.TotalPrice= basket.TotalPrice;
            await _publishEndpoint.Publish(eventmsg);

            _logger.LogInformation($"Basket Published for {basket.UserName}");
            //remove from basket
            var deletecmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(deletecmd);
            return Accepted();
        }

        [HttpPost("CheckoutV2")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutV2([FromBody] ShoppingCart basketCheckout)
        {
            if (basketCheckout == null || string.IsNullOrEmpty(basketCheckout.UserName))
                return BadRequest();

            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            
            decimal totalPrice = 0;
            if (basket != null && basket.Items != null && basket.Items.Count > 0)
            {
                totalPrice = basket.Items.Sum(x => x.Price * x.Quantity);
            }

            var eventmsg = new BasketCheckoutEventV2
            {
                UserName = basketCheckout.UserName,
                TotalPrice = totalPrice > 0 ? totalPrice : 50
            };

            await _publishEndpoint.Publish(eventmsg);
            _logger.LogInformation($"Basket Checkout V2 Event Published for {basketCheckout.UserName}");

            var deletecmd = new DeleteBasketByUserNameCommand(basketCheckout.UserName);
            await _mediator.Send(deletecmd);

            return Accepted();
        }



    }
}
