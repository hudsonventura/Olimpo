using Microsoft.AspNetCore.Mvc;
using Olimpo;
using Olimpo.Domain;

namespace server.Controllers;

[Route("[controller]")]
public class ChannelController : ControllerBase
{
    Context db;
    public ChannelController(Context db)
    {
        this.db = db;
    }


    [HttpPut("/channel/{id}")]
    public ActionResult Update(Guid id, [FromBody] Channel? channel){
        try
        {
            var channel_db = db.channels.Where(x => x.id == channel.id).FirstOrDefault();
            db.Entry(channel_db).CurrentValues.SetValues(channel);
            db.Update(channel_db);
            db.SaveChanges();
            
            return Ok(channel_db);
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }
}
