using Microsoft.AspNetCore.Mvc;
using Olimpo;
using Olimpo.Domain;

namespace server.Controllers;

[ApiController]
public class DeviceController : ControllerBase
{
    Context db;

    public DeviceController(Context db)
    {
        this.db = db;
    }

    [HttpPost("/device/")]
    public ActionResult Create(Device? device){
        try
        {
            db.devices.Add(device);
            db.SaveChanges();
            
            return Created($"/device/{device.id}", device);
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }


    [HttpPut("/device/{id}")]
    public ActionResult Update(Guid id, Device? device){
        try
        {
            var device_db = db.devices.Where(x => x.id == device.id).FirstOrDefault();
            db.Entry(device_db).CurrentValues.SetValues(device);
            db.Update(device_db);
            db.SaveChanges();
            
            return Ok(device_db);
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }
    
}
