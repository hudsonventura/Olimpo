using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Olimpo.Domain;
using Olimpo.Sensors;

namespace Olimpo;

public class SensorsChecker
{
    public static void StartLoopChecker(IConfiguration appsettings)
    {
        using (var db = new Context(appsettings)){
            List<Stack> stacks = db.stacks
            .Include(x => x.services)
            .ThenInclude(x => x.sensors)
            .ThenInclude(x => x.channels)
            .ToList();

            foreach (var stack in stacks)
            {
                foreach (var service in stack.services)
                {
                    foreach (var sensor in service.sensors)
                    {
                        Task task = Task.Run(() => SensorsChecker.LoopCheck(appsettings, sensor, service));
                        //Task.WaitAll(task);
                    }
                }
                
            }
        }

        
    }

    public static async void LoopCheck(IConfiguration appsettings, Sensor sensor, Service service)
    {
        string targetNamespace = "Olimpo.Sensors";
        string targetAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        string nomeCompletoDaClasse = $"{targetNamespace}.{sensor.type.ToUpper()}";
        Assembly assembly = Assembly.Load(targetAssemblyName);


        var  fullClassName  = assembly.GetType(nomeCompletoDaClasse);
        Type type = fullClassName;
        object instance;

        try
        {
            instance = Activator.CreateInstance(type);
        }
        catch (System.Exception error)
        {
            Metric metric_fake = new Metric(){
                id = Guid.NewGuid(),
                value = -1,
                datetime = DateTime.Now,
                error_code = 3,
                message = $"The type {sensor.type.ToString()} was not implemented yet"
            };
            Channel channel_fake = new Channel(){
                name = "Not checked",
                current_metric = metric_fake
            };
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
                    id = Guid.NewGuid(),
                    value = -1,
                    datetime = DateTime.Now,
                    error_code = 3,
                    message = $"The interface ISensor was not implemented correctly in the class {sensor.type.ToString()}"
                }
            };

            sensor.channels.Add(channel_fake);
            return;
        }

      

        using (var db = new Context(appsettings)){
            while(true){
                var new_channels = await GetMetric(sensor, service, instance, testMethod);
                foreach (var new_channel in new_channels)
                {
                    try
                    {
                        Sensor sensor_db = db.sensors.Where(x => x.id == sensor.id).Include(x => x.channels).ThenInclude(x => x.current_metric).FirstOrDefault();
                        Channel channel = sensor_db.channels.Where(x => x.channel_id == new_channel.channel_id)//.Include(x => x.current_metric)
                        .FirstOrDefault();

                        //the channel does not exists in the database, so, CREATE it
                        if(channel == null){
                            sensor_db.channels.Add(new_channel);
                        }

                        //the channel does already exists in the database, so, UPDATE it
                        if(channel != null){
                            channel.current_metric = new_channel.current_metric;
                            channel.metrics.Add(new_channel.current_metric);
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



    private static async Task<List<Channel>> GetMetric(Sensor sensor, Service service, object instance, MethodInfo method){
        var channels = sensor.channels;
        try
        {
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
                    error_code = 5,
                    datetime = DateTime.Now,
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
                    error_code = 5,
                    datetime = DateTime.Now,
                };
            });
            return channels;
        }
        return channels;
    }
}
