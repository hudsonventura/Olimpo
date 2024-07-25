using System.Reflection;
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
    public ActionResult Create(string id_device, Sensor? sensor){
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

    [HttpDelete("/sensor/{id}")]
    public ActionResult Delete(string id){
        try
        {
            var sensor_db = db.sensors.Where(x => x.id == id)
                                .Include(s => s.channels)
                                .ThenInclude(c => c.metrics) // Inclui entidades relacionadas
                            .FirstOrDefault(x => x.id == id);
            db.Remove(sensor_db);
            db.SaveChanges();
            
            return Ok(true);
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }

    [HttpGet("/sensor/types")]
    public ActionResult Types(){
        try
        {
            Dictionary<string, string> types = new Dictionary<string, string>();
            string namespaceParaProcurar = "Olimpo.Sensors";
            string funcaoParaExecutar = "GetType";

            // Obtenha todas as classes do namespace especificado
            var classesNoNamespace = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && t.Namespace == namespaceParaProcurar);

            foreach (var classe in classesNoNamespace)
            {
                try
                {
                    // Crie uma instância da classe
                    var instancia = Activator.CreateInstance(classe);

                    // Obtenha o método desejado
                    var metodo = classe.GetMethod(funcaoParaExecutar);

                    if (metodo != null)
                    {
                        // Execute o método
                        var retorno = metodo.Invoke(instancia, null);
                        if (retorno is string resultado)
                        {
                            types.Add(classe.Name, retorno.ToString());
                        }
                    }
                }
                catch (System.Exception)
                {
                    
                }
                
            }
            return Ok(types.ToList());
        }
        catch (System.Exception error)
        {
            return BadRequest(error);
        }
    }
    
}
