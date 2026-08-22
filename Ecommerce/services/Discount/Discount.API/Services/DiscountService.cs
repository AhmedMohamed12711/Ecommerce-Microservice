using Discount.Application.Commands;
using Discount.Application.Queries;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService:DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;
        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var result=await _mediator.Send(query);
            return result;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new CreateDiscountCommand{
                Amount=request.Coupon.Amount,
                ProductName=request.Coupon.ProductName,
                Description=request.Coupon.Description, 
            };
            var result=await _mediator.Send(cmd);
            return result;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDicountRequest request, ServerCallContext context)
        {
            var cmd = new UpdateDiscountCommand
            {
                Amount = request.Coupon.Amount,
                ProductName = request.Coupon.ProductName,
                Description = request.Coupon.Description,
                Id=request.Coupon.Id
            };
            var result = await _mediator.Send(cmd);
            return result;
        }

        public override async Task<DeleteDicountResponse> DeleteDiscount(DeleteDicountRequest request, ServerCallContext context)
        {
            var cmd = new DeleteDiscountCommand(request.ProductName);
            var deleted = await _mediator.Send(cmd);
            var respone = new DeleteDicountResponse
            {
                Success = deleted
            };
            return respone;
        }
    }
}
