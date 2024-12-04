using Order.Domain.Entities;
using SharedKernel;

namespace Order.Application.Commands;

public class AddOrderCommand : ICommand
{
    public Orders Order { get; set; }
}
public class AddUpdateOrdersCommandHandler : ICommandHandler<AddOrderCommand>
{
    private readonly IRepositoryBase<Orders> _order;    


    public AddUpdateOrdersCommandHandler(IRepositoryBase<Orders> order)
    {
        _order = order;        
    }

    public async Task Handle(AddOrderCommand command, CancellationToken cancellationToken = default)
    {        
        await _order.AddAsync(command.Order);        
    }
}
