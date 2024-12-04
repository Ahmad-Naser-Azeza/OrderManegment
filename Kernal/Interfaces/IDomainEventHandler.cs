﻿namespace SharedKernel;

public interface IDomainEventHandler<T>
              where T : IDomainEvent
{
    Task HandleAsync(T domainEvent, CancellationToken cancellationToken = default);
}