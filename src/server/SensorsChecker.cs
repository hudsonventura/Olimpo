using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Olimpo.Domain;
using Olimpo.Sensors;
using SnowflakeID;

namespace Olimpo;

public class SensorsChecker
{
    public static void StartLoopChecker(IConfiguration appsettings)
    {
        Dictionary<string, (Sensor, Task, CancellationTokenSource)> manager_sensors = new Dictionary<string, (Sensor, Task, CancellationTokenSource)>();
        using (var db = new Context(appsettings)){
            while(true){
                List<Stack> stacks = db.stacks
                    .Include(x => x.devices)
                    .ThenInclude(x => x.sensors)
                    .ThenInclude(x => x.channels)
                    .ToList();

                foreach (var stack in stacks)
                {
                    foreach (var service in stack.devices)
                    {
                        foreach (var sensor in service.sensors)
                        {

                            //if the sensor was not running yet
                            if (!manager_sensors.ContainsKey(sensor.id))
                            {
                                var cancel = new CancellationTokenSource();
                                CancellationToken cancelation_token = cancel.Token;

                                Task task = Task.Run(() => SensorsChecker.LoopCheck(appsettings, sensor, service), cancelation_token);
                                
                                manager_sensors.Add(sensor.id, (sensor, task, cancel));
                                continue;
                            }

                            //the sensor exists, but it's different or it was updated
                            if(manager_sensors.ContainsKey(sensor.id) && manager_sensors[sensor.id].Item1 != sensor){
                                CancellationTokenSource cancellation = manager_sensors[sensor.id].Item3;
                                cancellation.Cancel();
                                manager_sensors.Remove(sensor.id);
                            }
                        }
                    }
                }
                Thread.Sleep(60000);
            }
        }
    }
    
    

    public static async void LoopCheck(IConfiguration appsettings, Sensor sensor, Device service)
    {
        string targetNamespace = "Olimpo.Sensors";
        string targetAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        string nomeCompletoDaClasse = $"{targetNamespace}.{sensor.type}";
        Assembly assembly = Assembly.Load(targetAssemblyName);


        var  fullClassName  = assembly.GetTypes().FirstOrDefault(t => t.FullName.Equals(nomeCompletoDaClasse, StringComparison.OrdinalIgnoreCase));
        if(fullClassName == null){
            Console.WriteLine($"The sensor {nomeCompletoDaClasse} not exists");
            return;
        }
        Type type = fullClassName;
        object instance;

        try
        {
            instance = Activator.CreateInstance(type);
        }
        catch (System.Exception error)
        {
            Channel channel_fake = new Channel(){
                name = "Not checked",
                current_metric = new Metric(){
                    id = SnowflakeIDGenerator.GetSnowflake(0),
                    status = Metric.Status.Error,
                    message = $"The type {sensor.type.ToString()} was not implemented yet"
                }};
            sensor.channels.Add(channel_fake);
            return;
        }

        MethodInfo testMethod;
        try
        {
            testMethod = type.GetMethod("Test");
        }
        catch (System.Exception)
        {
            Channel channel_fake = new Channel(){
                name = "Not checked",
                current_metric = new Metric(){
                    id = SnowflakeIDGenerator.GetSnowflake(0),
                    status = Metric.Status.Error,
                    message = $"The interface ISensor was not implemented correctly in the class {sensor.type.ToString()}"
                }
            };

            sensor.channels.Add(channel_fake);
            return;
        }

      

        using (var db = new Context(appsettings)){
            while(true){
                var db_sensor = db.sensors.Where(x => x.id == sensor.id).Include(x => x.channels).AsNoTracking().FirstOrDefault();
                var new_channels = await GetMetric(db_sensor, service, instance, testMethod);
                if(new_channels == null){
                    break;
                }
                
                foreach (var new_channel in new_channels)
                {
                    try
                    {
                        Sensor sensor_db = db.sensors.Where(x => x.id == sensor.id).Include(x => x.channels).ThenInclude(x => x.current_metric).FirstOrDefault();
                        Channel channel = sensor_db.channels.Where(x => x.channel_id == new_channel.channel_id)//.Include(x => x.current_metric)
                        .FirstOrDefault();

                        

                        //the channel does not exists in the database, so, CREATE it
                        if(channel == null){
                            var (status, msg) = DetermineStatus(new_channel);
                            new_channel.current_metric.status = status;
                            new_channel.current_metric.message = msg;
                            sensor_db.channels.Add(new_channel);
                        }

                        //the channel does already exists in the database, so, UPDATE it
                        if(channel != null){
                            channel.current_metric = new_channel.current_metric;
                            var (status, msg) = DetermineStatus(channel);
                            channel.current_metric.status = status;
                            channel.current_metric.message = msg;
                            //channel.metrics.Add((Metric_History)new_channel.current_metric); //TODO: adicionar o historico
                        }

                        db.SaveChanges();

                    }
                    catch (System.Exception error)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Error on save metric");
                        Console.WriteLine($"Sensor: {sensor.name}");
                        Console.WriteLine($"Channel: {new_channel.name}");
                        Console.WriteLine($"Exception: {error.Message}");
                        if (error.InnerException != null)
                        {
                            Console.WriteLine($"InnerException: {error.InnerException.Message}");
                        }
                        
                    }
                }
                //sensor.channels = new_channels;
                Thread.Sleep(sensor.check_each);
            }
            
        }
    }




    private static (Metric.Status, string) DetermineStatus(Channel channel)
    {

        if(channel.current_metric.status == Metric.Status.Offline){
            return (Metric.Status.Offline, channel.current_metric.message);
        }
        
        var status_danger = DetermineStatus_2((decimal) channel.current_metric.value, channel.danger_value, channel.danger_orientation);
        if(status_danger){
            var msg = $"The value is worst than {channel.danger_value}{channel.unit}";
            return (Metric.Status.Error, msg);
        }

        var status_warning = DetermineStatus_2((decimal) channel.current_metric.value, channel.warning_value, channel.warning_orientation);
        if(status_warning){
            var msg = $"The value is worst than {channel.warning_value}{channel.unit} but better than {channel.danger_value}{channel.unit}";
            return (Metric.Status.Warning, msg);
        }

        var status_success = DetermineStatus_2((decimal) channel.current_metric.value, channel.success_value, channel.success_orientation);
        if(status_success){
            var msg = $"The value is better than {channel.success_value}{channel.unit}";
            return (Metric.Status.Success, msg);
        }

        return (Metric.Status.Success, channel.current_metric.message);
    }

    private static bool DetermineStatus_2(decimal value, decimal base_value, Channel.Orientation orientation){
        if(orientation == Channel.Orientation.Disabled){
            return false;
        }

        switch(orientation){
            case Channel.Orientation.GreaterThan:           return (value >  base_value) ? true : false;
            case Channel.Orientation.GreaterThanOrEqual:    return (value >= base_value) ? true : false;
            case Channel.Orientation.Equal:                 return (value == base_value) ? true : false;
            case Channel.Orientation.LessThanOrEqual:       return (value <= base_value) ? true : false;
            case Channel.Orientation.LessThan:              return (value <  base_value) ? true : false;
            default:                                        return true;
        }
    }





    private static async Task<List<Channel>> GetMetric(Sensor sensor, Device service, object instance, MethodInfo method){
        List<Channel> channels = null;
        try
        {
            channels = sensor.channels;
            var result_task = method.Invoke(instance, new object[] { service, sensor });
            // Converte o retorno para o tipo esperado, por exemplo, string
            if (result_task is Task task)
            {
                await task; // Aguarda a conclusão da tarefa

                // Se o método retornar Task<T>, você pode acessar o resultado com reflection
                if (result_task.GetType().IsGenericType)
                {
                    var resultProperty = result_task.GetType().GetProperty("Result");
                    return (List<Channel>)resultProperty.GetValue(result_task);
                }
            }
        }
        catch (TargetInvocationException error)
        {
            channels.ForEach(x => {
                x.current_metric = new Metric(){
                    message = error.Message,
                    status = Metric.Status.Error,
                    datetime = DateTime.UtcNow,
                };
            });
            return channels;
        }
        catch (Exception error)
        {
            if(channels == null || channels.Count() == 0){
                return channels;
            }
            channels.ForEach(x => {
                x.current_metric = new Metric(){
                    message = error.Message,
                    status = Metric.Status.Error,
                    datetime = DateTime.UtcNow,
                };
            });
            return channels;
        }
        return channels;
    }
}
