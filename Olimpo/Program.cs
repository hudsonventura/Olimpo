using Olimpo;
using Olimpo.Protocols;
using Olimpo.Domain;

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Spectre.Console;


public class Program
{
    static IConfiguration appsettings;
    static List<Ativo> ativos;

    static (Ativo, Result)[] results;

    static Layout layout;

    public static async Task Main(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .Build();

        var stacks = appsettings.GetSection("stacks").Get<List<Stack>>();


        //star threads to check each sensor
        foreach (var stack in stacks)
        {
            foreach (var service in stack.services)
            {
                foreach (var sensor in service.sensors)
                {
                    Task task = Task.Run(() => LoopMetricCheck(sensor, service));
                    Task.WaitAll(task);
                }
            }
            
        }

        while(true){
            Thread.Sleep(1000);
        }




        // Create the layout
        layout = new Layout("Root")
            .SplitColumns(
                new Layout("Stacks").Ratio(9),
                new Layout("Right")
                    .SplitRows(
                        new Layout("Top"),
                        new Layout("Bottom")));
    


        await AnsiConsole.Live(layout)
            .StartAsync(async ctx => 
            {
                while(true){
                    //ShowTable(stacks);
                    ctx.Refresh();
                    await Task.Delay(1000);   
                }
                      
            });
        

    }

    private static async void LoopMetricCheck(Sensor sensor, Service service)
    {
        string targetNamespace = "Olimpo.Protocols";
        string targetAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;;
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
            sensor.metrics.Add(new Metric(){
                id = Guid.NewGuid(),
                value = -1,
                datetime = DateTime.Now,
                error_code = 1,
                message = $"The type {sensor.type.ToString()} was not implemented yet"
            });
            return;
        }

        MethodInfo testMethod;
        MethodInfo unitMethod;
        try
        {
            testMethod = type.GetMethod("Test");
            unitMethod = type.GetMethod("GetUnit");
        }
        catch (System.Exception)
        {
            sensor.metrics.Add(new Metric(){
                id = Guid.NewGuid(),
                value = -1,
                datetime = DateTime.Now,
                error_code = 3,
                message = $"The method Test was not implemented at class {sensor.type.ToString()}"
            });
            return;
        }

        
        

        while(true){
            Metric metric = await GetMetric(sensor, service, instance, testMethod);
            metric.unit = GetUnit(instance, unitMethod);
            sensor.metrics.Add(metric);
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}  - Checked service '{service.name}' sensor '{sensor.name}': Value {metric.value}{metric.unit} and Latency {metric.latency}ms");
            Thread.Sleep(sensor.check_each);
        }
    }

    private static string GetUnit(object instance, MethodInfo? method)
    {
        var result_task = method.Invoke(instance, new object[] {  });
        return result_task.ToString();
    }

    private static async Task<Metric> GetMetric(Sensor sensor, Service service, object instance, MethodInfo method){
        
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
                    dynamic result = resultProperty.GetValue(result_task);

                    return new Metric(){
                        id = Guid.NewGuid(),
                        value = (decimal) result.Value,
                        latency = result.Latency,
                        datetime = DateTime.Now,
                        error_code = 0,
                        message = "Success"
                    };
                }
            }
            return new Metric(){
                id = Guid.NewGuid(),
                value = -1,
                datetime = DateTime.Now,
                error_code = 6,
                message = "I can't get the correct value"
            };
        }
        catch (TargetInvocationException error)
        {
            return new Metric(){
                id = Guid.NewGuid(),
                value = -1,
                datetime = DateTime.Now,
                error_code = 5,
                message = error.Message
            };
        }
        catch (Exception error)
        {
            return new Metric(){ 
                id = Guid.NewGuid(),
                value = -1,
                datetime = DateTime.Now,
                error_code = 5,
                message = error.Message
            };
        }
        
    }





















    static async void ShowTable(List<Stack> stacks)
    {
        Tree root = new Tree("Stacks");

        foreach (var stack in stacks)
        {
            var stackNode = root.AddNode("[bold][blue][[Titulo Stack]][/][/]");

            var grid = new Grid();
        
            // Add columns 
            grid.AddColumn().Width(150);
            grid.AddColumn().Width(200);
            grid.AddColumn().Width(50);
            grid.AddColumn().Width(50);
            grid.AddColumn().Width(70);
            grid.AddColumn().Width(400);

            foreach (var service in stack.services)
            {
                // Add header row 
                grid.AddRow(new Text[]{
                    new Text("Service", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Host", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Protocol", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Port", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Latency", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Status", new Style(Color.Blue, Color.Black)).LeftJustified()
                });

                foreach (var sensor in service.sensors)
                {
                    Metric metric = sensor.metrics.LastOrDefault();

                    var color = (metric.value == -1) ? "red" : metric.value < 50 ? new Style(Color.Green) : metric.value < 100 ? new Style(Color.Yellow) : new Style(Color.Red);
                    var value = (metric.value == -1) ? "-" : $"{metric.value.ToString()}{metric.unit}";
                
                    // Add content row 
                    grid.AddRow(new Text[]{
                        new Text(sensor.name).LeftJustified(),
                        new Text(service.host).LeftJustified(),
                        new Text(sensor.type.ToString()).LeftJustified(),
                        new Text(sensor.port.ToString()).LeftJustified(),
                        new Text(value, color).LeftJustified(),
                        new Text(metric.message, color).LeftJustified(),
                    });
                }
                stackNode.AddNode(grid);
            }
            // Update the Stacks column
            layout["Stacks"].Update(
                new Panel(root)
                    .Expand());
        }

        

        


 

        
    }


}

