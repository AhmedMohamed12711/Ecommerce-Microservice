using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entites;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers.Commands;

public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
{
    private readonly IDiscountRepository DiscountRepository;
    private readonly IMapper Mapper;

    public UpdateDiscountCommandHandler(IDiscountRepository discountRepository, IMapper mapper)
    {
        DiscountRepository = discountRepository;
        Mapper = mapper;
    }
    public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
    {
        var coupon = Mapper.Map<Coupon>(request);
        await DiscountRepository.UpdateDiscount(coupon);
        var couponModel=Mapper.Map<CouponModel>(coupon);
        return couponModel;
    }
}
