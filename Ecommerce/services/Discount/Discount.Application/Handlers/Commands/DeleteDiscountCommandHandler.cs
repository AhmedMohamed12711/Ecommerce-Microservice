using AutoMapper;
using Discount.Application.Commands;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Handlers.Commands;

public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
{
    private readonly IDiscountRepository DiscountRepository;

    public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
    {
        DiscountRepository = discountRepository;
    }
    public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var deleted = await DiscountRepository.DeleteDiscount(request.ProductName);
        return deleted;
    }
}
