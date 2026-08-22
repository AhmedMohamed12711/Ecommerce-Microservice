
using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands;

public class DeleteDiscountCommand:IRequest<bool>
{
    public string ProductName { get; set; } = null!;
    public DeleteDiscountCommand(string productName)
    {
        ProductName = productName;
    }
}
