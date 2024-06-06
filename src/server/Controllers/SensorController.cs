using Microsoft.AspNetCore.Mvc;
using Olimpo;
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

    [HttpPost("/sensor/")]
    public ActionResult Create(Sensor? sensor){
        try
        {
            db.sensors.Add(sensor);
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
