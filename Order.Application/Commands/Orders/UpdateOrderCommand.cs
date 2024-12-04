using Order.Domain.Entities;
using SharedKernel;

namespace Order.Application.Commands;

public class UpdateOrderCommand : ICommand
{
    public Orders Order { get; set; }
}
public class UpdateUpdateOrdersCommandHandler : ICommandHandler<UpdateOrderCommand>
{
    private readonly IRepositoryBase<Orders> _order;    


    public UpdateUpdateOrdersCommandHandler(IRepositoryBase<Orders> order)
    {
        _order = order;        
    }

    public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken = default)
    {        
        await _order.UpdateAsync(command.Order);        
    }
}
