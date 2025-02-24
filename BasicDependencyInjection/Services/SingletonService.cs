﻿namespace BasicDependencyInjection.Services;

public class SingletonService : ISingletonService
{
    private readonly DateTime _createdAt;
    private readonly Guid _serviceId;

    public SingletonService()
    {
        _serviceId = Guid.NewGuid();
        _createdAt = DateTime.Now;
    }

    public string Name => nameof(SingletonService);

    public string SayHello()
    {
        return $"Hello! I am {Name}. My Id is {_serviceId}. I was created at {_createdAt:yyyy-MM-dd HH:mm:ss}.";
    }
}