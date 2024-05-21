using Olimpo;
using Olimpo.Sensors;
using Olimpo.Domain;

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Spectre.Console;


public class Program
{
    static List<Stack> stacks;
    public static async Task Main(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        IConfiguration appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .Build();

        stacks = appsettings.GetSection("stacks").Get<List<Stack>>();


        //star threads to check each sensor
        foreach (var stack in stacks)
        {
            foreach (var service in stack.services)
            {
                foreach (var sensor in service.sensors)
                {
                    Task task = Task.Run(() => LoopMetricCheck(sensor, service));
                    //Task.WaitAll(task);
                }
            }
            
        }




        


        // Create the layout
        Layout layout = new Layout("Root")
            .SplitColumns(
                new Layout("Stacks").Ratio(9).SplitRows(
                        new Layout("Top"),
                        new Layout("Bottom")));
                    
        layout["Bottom"].Size(3);
        layout["Bottom"].Update(
            new Panel(
                Align.Center(
                    new Markup("Hello [blue]World![/]")
                )
            )
        );

        while(true){
            ShowConsole(layout);
            Thread.Sleep(3000);
        }

    }

    private static async void LoopMetricCheck(Sensor sensor, Service service)
    {
        string targetNamespace = "Olimpo.Sensors";
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
            //Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}  - Checked service '{service.name}' sensor '{sensor.name}': Value {metric.value}{metric.unit} and Latency {metric.latency}ms");
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
                        message = result.Message
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





















    static async void ShowConsole(Layout layout)
    {
        Tree root = new Tree("Stacks");
        

        foreach (var stack in stacks)
        {
            var stackNode = root.AddNode($"[bold][blue][[{stack.name}]][/][/]");

            
        
            

            foreach (var service in stack.services)
            {
                

                // Create sendor grid 
                var grid = new Grid();
                grid.AddColumn().Width(150);
                grid.AddColumn().Width(200);
                grid.AddColumn().Width(50);
                grid.AddColumn().Width(50);
                grid.AddColumn().Width(70);
                grid.AddColumn().Width(70);
                grid.AddColumn().Width(400);

                // Add header row 
                grid.AddRow(new Text[]{
                    new Text("Sensor", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Host", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Protocol", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Port", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Latency", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Value", new Style(Color.Blue, Color.Black)).LeftJustified(),
                    new Text("Status", new Style(Color.Blue, Color.Black)).LeftJustified()
                });
                
                foreach (var sensor in service.sensors)
                {
                    

                    Metric metric = sensor.metrics.LastOrDefault();
                    if(metric == null){
                        metric = new Metric(){
                            datetime = DateTime.Now
                        };
                    }

                    Style color = Color.Green;
                    color = (metric.value == -1) ? new Style(Color.Red) : metric.value > 100 ? new Style(Color.Red) : metric.value >= 50 ? new Style(Color.Yellow) : new Style(Color.Green);

                    if(sensor.alerts != null){
                        switch (sensor.alerts.type)
                        {
                            case Alert.Type.exact: color = (metric.value == sensor.alerts.critical) ? new Style(Color.Red) : metric.value == sensor.alerts.warning ? new Style(Color.Yellow) : new Style(Color.Green);
                            break;

                            //TODO: implementar lower_better
                            case Alert.Type.lower_better: color = (metric.value == -1) ? new Style(Color.Red) : metric.value <= sensor.alerts.success ? new Style(Color.Green) : metric.value <= sensor.alerts.warning ? new Style(Color.Yellow) : new Style(Color.Red);
                            break;

                            //TODO: implementar upper_better
                            case Alert.Type.upper_better: color = (metric.value == -1) ? new Style(Color.Red) : metric.value < 50 ? new Style(Color.Red) : metric.value < 100 ? new Style(Color.Yellow) : new Style(Color.Red);
                            break;

                            default: break;
                        }
                    }
                    
                    var value = (metric.value == -1) ? "-" : metric.value.ToString();
                
                    // Add content row 
                    grid.AddRow(new Text[]{
                        new Text(sensor.name).LeftJustified(),
                        new Text(".").LeftJustified(),
                        new Text(sensor.type.ToString()).LeftJustified(),
                        new Text(sensor.port.ToString()).LeftJustified(),
                        new Text(metric.latency.ToString()+"ms").LeftJustified(),
                        new Text(value.ToString()+metric.unit, color).LeftJustified(),
                        new Text(metric.message, color).LeftJustified(),
                    });
                }
                var serviceNode = stackNode.AddNode($"{service.name} - {service.host}");
                serviceNode.AddNode(grid);
            }
            ;
            // Update the Stacks column
            layout["Top"].Update(
                 new Panel(root)
                     .Expand());
            AnsiConsole.Write(layout);
        }

        

        


 

        
    }


}

