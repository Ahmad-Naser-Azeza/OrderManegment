using MediatR;

namespace MediatorCoordinator.Contract;

public interface IRequestContext<out TResponse> : IRequest<TResponse>, IRequestContext { }

public interface IRequestContext { }