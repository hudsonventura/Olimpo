using Microsoft.AspNetCore.Mvc;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class APIController : ControllerBase
{

    public APIController(IConfiguration appsettings)
    {
        
    }

    [HttpGet("/Api/")]
    public ActionResult Get()
    {
        return Ok();
    }
}