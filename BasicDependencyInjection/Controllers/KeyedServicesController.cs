using BasicDependencyInjection.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicDependencyInjection.Controllers;

[ApiController]
[Route("[controller]")]
public class KeyedServicesController : ControllerBase
{
    [HttpGet("sql")]
    public ActionResult GetSqlData([FromKeyedServices("sqlDatabaseService")] IDataService dataService)
    {
        return Content(dataService.GetData());
    }

    [HttpGet("cosmos")]
    public ActionResult GetCosmosData([FromKeyedServices("cosmosDatabaseService")] IDataService dataService)
    {
        return Content(dataService.GetData());
    }
}