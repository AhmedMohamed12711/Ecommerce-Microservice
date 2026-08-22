using Asp.Versioning;
using Catalog.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System.Net;

namespace Ordering.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : BaseApiController
    {
        private readonly IMediator _mediator;

        private readonly ILogger<OrdersController> _logger;
        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        [HttpGet("{userName}",Name ="GetOrderByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrderByUserName(string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders =await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpPost( Name = "CheckoutOrder")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result=await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder (int id)
        {
            var result = await _mediator.Send(new DeleteOrderCommand() { Id = id });
            return NoContent();
        }
    }
}
