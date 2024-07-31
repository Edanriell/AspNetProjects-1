namespace BasicDependencyInjection.Services;

public class DemoService : IDemoService
{
    private readonly Guid _serviceId;
    private readonly DateTime _createdAt;

    public DemoService()
    {
        _serviceId = Guid.NewGuid();
        _createdAt = DateTime.Now;
    }

    public string SayHello()
    {
        return $"Hello! My Id is {_serviceId}. I was created at {_createdAt:yyyy-MM-ddHH:mm:ss}.";
    }
}