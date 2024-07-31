namespace BasicDependencyInjection.Services;

public interface IService
{
    string Name { get; }
    string SayHello();
}