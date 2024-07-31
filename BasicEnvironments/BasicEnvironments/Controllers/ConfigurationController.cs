using Microsoft.AspNetCore.Mvc;

namespace BasicEnvironments.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationController(IConfiguration configuration) : ControllerBase
{
    [HttpGet]
    [Route("database-configuration")]
    public ActionResult GetDatabaseConfiguration()
    {
        var type = configuration["Database:Type"];
        var connectionString = configuration["Database:ConnectionString"];
        return Ok(new { Type = type, ConnectionString = connectionString });
    }
}