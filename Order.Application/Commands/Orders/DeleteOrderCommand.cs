using Order.Domain.Entities;
using SharedKernel;

namespace Order.Application.Commands;

public class DeleteOrderCommand : ICommand
{
    public required Orders Order { get; set; }
}

public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
{
    public readonly IRepositoryBase<Orders> _repository;    

    public DeleteOrderCommandHandler(IRepositoryBase<Orders> repository)
    {
        _repository = repository;        
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Order, cancellationToken);        

    }
}
