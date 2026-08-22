using Discount.Grpc.Protos;

namespace Basket.Application.GrpcServices;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _dicountgrpcClient;
    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient dicountgrpcClient)
    {
        _dicountgrpcClient=dicountgrpcClient;
    }
    public async Task<CouponModel> GetDiscount(string productName)
    {
        var discountRequest=new GetDiscountRequest { ProductName=productName};
        return await _dicountgrpcClient.GetDiscountAsync(discountRequest);
    }
}
