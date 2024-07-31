using BasicDependencyInjection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicDependencyInjection.Controllers;

[ApiController]
[Route("[controller]")]
public class LifetimeController : ControllerBase
{
    private readonly IScopedService _scopedService;

    //private readonly ITransientService _transientService;
    private readonly ISingletonService _singletonService;

    public LifetimeController(IScopedService scopedService, ITransientService transientService,
        ISingletonService singletonService)
    {
        _scopedService = scopedService;
        //_transientService = transientService;
        _singletonService = singletonService;
    }

    [HttpGet]
    public ActionResult Get([FromServices] ITransientService transientService)
    {
        var scopedServiceMessage = _scopedService.SayHello();
        var transientServiceMessage = transientService.SayHello();
        var singletonServiceMessage = _singletonService.SayHello();
        return Content(
            $"{scopedServiceMessage}{Environment.NewLine}{transientServiceMessage}{Environment.NewLine}{singletonServiceMessage}");
    }
}