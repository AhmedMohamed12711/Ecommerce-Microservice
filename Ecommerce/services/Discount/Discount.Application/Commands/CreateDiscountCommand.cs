using Discount.Core.Entites;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands;

public class CreateDiscountCommand:IRequest<CouponModel>
{
    public string ProductName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Amount { get; set; }

}
