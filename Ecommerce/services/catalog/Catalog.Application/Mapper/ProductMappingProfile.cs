using AutoMapper;
using Catalog.Application.Responses;
using Catalog.Core.Entites;
using Catalog.Core.Specs;

namespace Catalog.Application.Mapper;

public class ProductMappingProfile:Profile
{
    public ProductMappingProfile()
    {
        CreateMap<ProductBrand,BrandResponseDto>().ReverseMap();
        CreateMap<Product,ProductResponseDto>().ReverseMap();
        CreateMap<ProductType,TypeResponseDto>().ReverseMap();
        CreateMap<Pagination<Product>,Pagination<ProductResponseDto>>().ReverseMap();

    }
}
