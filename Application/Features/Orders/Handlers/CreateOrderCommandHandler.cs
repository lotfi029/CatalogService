using Application.Features.Orders.Command;
using Domain.Enums;

namespace Application.Features.Orders.Handlers;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    public Task<Result<Guid>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            ProductId = command.ProductId,
            Quantity = command.Request.Quantity,
            Price = command.Request.Price,
            TotalPrice = command.Request.Quantity * command.Request.Price,
            Status = OrderStatus.Processing
        };
        // check the quantity of the product
        // check the price of the product
        // check the user's balance
        // check the user's address
        // check the user's phone number
        // check the 

        throw new NotImplementedException();
    }
}
