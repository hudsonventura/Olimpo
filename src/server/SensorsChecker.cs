using System.Reflection;
using Olimpo.Domain;
using Olimpo.Sensors;

namespace Olimpo;

public class SensorsChecker
{
    public static void StartLoopChecker(List<Stack> stacks){
        foreach (var stack in stacks)
        {
            foreach (var service in stack.services)
            {
                foreach (var sensor in service.sensors)
                {
                    Task task = Task.Run(() => SensorsChecker.LoopCheck(sensor, service));
                    //Task.WaitAll(task);
                }
            }
            
        }
    }

    public static async void LoopCheck(Sensor sensor, Service service)
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
                metric = metric_fake
            };
            sensor.channels.Add(channel_fake);
            return;
        }

        MethodInfo testMethod;
        MethodInfo unitMethod;
        MethodInfo genChannelMethod;
        try
        {
            testMethod = type.GetMethod("Test");
            unitMethod = type.GetMethod("GetUnit");
            genChannelMethod = type.GetMethod("GenChannels");
        }
        catch (System.Exception)
        {
            Metric metric_fake = new Metric(){
                id = Guid.NewGuid(),
                value = -1,
                datetime = DateTime.Now,
                error_code = 3,
                message = $"The interface ISensor was not implemented correctly in the class {sensor.type.ToString()}"
            };
            Channel channel_fake = new Channel(){
                name = "Not checked",
                metric = metric_fake
            };
            sensor.channels.Add(channel_fake);
            return;
        }

        List<Olimpo.Domain.Channel> channels = (List<Olimpo.Domain.Channel>) genChannelMethod.Invoke(instance, new object[] { sensor });

        

        while(true){
            sensor.channels = channels;
            sensor = await GetMetric(sensor, service, instance, testMethod);
            Thread.Sleep(sensor.check_each);
        }
    }



    private static async Task<Sensor> GetMetric(Sensor sensor, Service service, object instance, MethodInfo method){
        
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
                    sensor = (Sensor)resultProperty.GetValue(result_task);
                    return sensor;
                }
            }
        }
        catch (TargetInvocationException error)
        {
            sensor.channels.ForEach(x => {
                x.metric = new Metric(){
                    message = error.Message,
                    error_code = 5,
                    datetime = DateTime.Now,
                };
            });
            return sensor;
        }
        catch (Exception error)
        {
            if(sensor.channels == null || sensor.channels.Count() == 0){
                return sensor;
            }
            sensor.channels.ForEach(x => {
                x.metric = new Metric(){
                    message = error.Message,
                    error_code = 5,
                    datetime = DateTime.Now,
                };
            });
            return sensor;
        }
        return sensor;
    }
}
