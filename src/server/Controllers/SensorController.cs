using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Olimpo;
using Olimpo.Domain;
using Olimpo.Sensors;

namespace server.Controllers;

[ApiController]
public class SensorController : ControllerBase
{
    Context db;

    public SensorController(Context db)
    {
        this.db = db;
    }

    [HttpPost("/sensor/{id_device}")]
    public ActionResult Create(Guid id_device, Sensor? sensor){
        try
        {
            Device device = db.devices.Where(x => x.id == id_device)
                            .Include(x => x.sensors)
                            .FirstOrDefault();
            device.sensors.Add(sensor);
            db.SaveChanges();

            return Created($"/sensor/{sensor.id}", sensor);
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }


    [HttpPut("/sensor/{id}")]
    public ActionResult Update(Guid id, Sensor? sensor){
        try
        {
            var sensor_db = db.sensors.Where(x => x.id == sensor.id).FirstOrDefault();
            db.Entry(sensor_db).CurrentValues.SetValues(sensor);
            db.Update(sensor_db);
            db.SaveChanges();
            
            return Ok(sensor_db);
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }
    
}
