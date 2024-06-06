using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Olimpo;
using Olimpo.Domain;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class APIController : ControllerBase
{
    Context db;

    public APIController(Context db)
    {
        this.db = db;
    }


    [HttpGet("/Api/")]
    public ActionResult Get()
    {
        var stacks = db.stacks
            .Include(x => x.devices)
            .ThenInclude(x => x.sensors)
            .ThenInclude(x => x.channels)
            .ThenInclude(x => x.current_metric)
            .OrderBy(x => x.order)
            .ThenBy(x => x.order)
            .ToList();

        return Ok(stacks);
    }
}
