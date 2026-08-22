using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.Queries;

public class GetAllTypeQueryHandler : IRequestHandler<GetAllTypeQuery, IList<TypeResponseDto>>
{
    private readonly IMapper _mapper;
    private readonly ITypeRepository _typeRepository;

    public GetAllTypeQueryHandler(IMapper mapper, ITypeRepository typeRepository)
    {
        _typeRepository = typeRepository;
        _mapper = mapper;
    }

    public async Task<IList<TypeResponseDto>> Handle(GetAllTypeQuery request, CancellationToken cancellationToken)
    {
        var TypeList = await _typeRepository.GetAllTypes();
        var TypeResponseList=_mapper.Map<IList<TypeResponseDto>>(TypeList);
        return TypeResponseList;
    }
}
