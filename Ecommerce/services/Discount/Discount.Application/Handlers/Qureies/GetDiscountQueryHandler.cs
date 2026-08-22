using Discount.Application.Queries;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Handlers.Qureies;

public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
{
    private readonly IDiscountRepository DiscountRepository;
    private readonly ILogger<GetDiscountQueryHandler> Logger;

    public GetDiscountQueryHandler(IDiscountRepository discountRepository, ILogger<GetDiscountQueryHandler> logger)
    {
        DiscountRepository=discountRepository;
        Logger=logger;
    }
    public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
    {
        var coupon = await DiscountRepository.GetDiscount(request.ProductName);
        if (coupon==null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount for this product {request.ProductName} not found"));
        }
        var couponModel=new CouponModel
        {
            Amount= coupon.Amount,
            Description= coupon.Description,
            ProductName= coupon.ProductName,
            Id= coupon.Id
        };
        Logger.LogInformation($"Coupon for this {request.ProductName} is fetched");
        return couponModel;
    }
}
