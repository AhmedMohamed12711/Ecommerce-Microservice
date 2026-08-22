using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Entites;
using Discount.Grpc.Protos;

namespace Discount.Application.Mapper;

public class Discountprofile:Profile
{
    public Discountprofile()
    {
        CreateMap<Coupon,CouponModel>().ReverseMap();
        CreateMap<CreateDiscountCommand, Coupon>().ReverseMap();
        CreateMap<UpdateDiscountCommand, Coupon>().ReverseMap();
    }
}
