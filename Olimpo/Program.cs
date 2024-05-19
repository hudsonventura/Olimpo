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
        ativos = appsettings.GetSection("ativos").Get<List<Ativo>>();
        results = new (Ativo, Result)[ativos.Count];
        for (int i = 0; i < ativos.Count; i++)
        {
            results[i] = (ativos[i], new Result(){ Message = "Not verified yet"});
        }

        var stacks = appsettings.GetSection("stacks").Get<List<Stack>>();

        foreach (var stack in stacks)
        {
            foreach (var service in stack.services)
            {
                foreach (var sensor in service.sensors)
                {
                    Task task = Task.Run(() => LoopMetricCheck(sensor, service, 1000));
                    Task.WaitAll(task);
                }
            }
            
        }

        while(true){
            Thread.Sleep(1000);
        }

        Task taskData = Task.Run(() => ObtainValues());



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
                    ShowTable();
                    ctx.Refresh();
                    await Task.Delay(1000);   
                }
                      
            });
        

    }

    private static async void LoopMetricCheck(Sensor sensor, Service service, int wait)
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

        MethodInfo method;
        try
        {
            method = type.GetMethod("Test");
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
            Console.WriteLine($"Check! {service.name}");
            Metric metric = await GetMetric(sensor, service, instance, method);
            sensor.metrics.Add(metric);
            Thread.Sleep(wait);
        }
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






















    static async void ObtainValues(){
        while(true){
            for (int i = 0; i < ativos.Count; i++)
            {
                var result = await CheckHost(ativos[i]);
                results[i] = (ativos[i], result);
            }
            await Task.Delay(3000);
        }
    }

    static async void ShowTable()
    {
        Tree root = new Tree("Stacks");

        var stack = root.AddNode("[bold][blue][[Titulo Stack]][/][/]");

        var grid = new Grid();
    
        // Add columns 
        grid.AddColumn().Width(150);
        grid.AddColumn().Width(200);
        grid.AddColumn().Width(50);
        grid.AddColumn().Width(50);
        grid.AddColumn().Width(70);
        grid.AddColumn().Width(400);

        // Add header row 
        grid.AddRow(new Text[]{
            new Text("Service", new Style(Color.Blue, Color.Black)).LeftJustified(),
            new Text("Host", new Style(Color.Blue, Color.Black)).LeftJustified(),
            new Text("Protocol", new Style(Color.Blue, Color.Black)).LeftJustified(),
            new Text("Port", new Style(Color.Blue, Color.Black)).LeftJustified(),
            new Text("Latency", new Style(Color.Blue, Color.Black)).LeftJustified(),
            new Text("Status", new Style(Color.Blue, Color.Black)).LeftJustified()
        });

        

        foreach (var (ativo, result) in results)
        {
            var color = (result.Latency == -1) ? "red" : result.Latency < 50 ? new Style(Color.Green) : result.Latency < 100 ? new Style(Color.Yellow) : new Style(Color.Red);
            var value = (result.Latency == -1) ? "-" : result.Latency.ToString()+"ms";
        
            // Add content row 
            grid.AddRow(new Text[]{
                new Text(ativo.name).LeftJustified(),
                new Text(ativo.host).LeftJustified(),
                new Text(ativo.type.ToString()).LeftJustified(),
                new Text(ativo.port.ToString()).LeftJustified(),
                new Text(value, color).LeftJustified(),
                new Text(result.Message, color).LeftJustified(),
            });
        }
        stack.AddNode(grid);
        
 

        // Update the Stacks column
        layout["Stacks"].Update(
            new Panel(root)
                .Expand());
    }

    static async Task<Result> CheckHost(Ativo ativo)
    {
        string targetNamespace = "Olimpo.Protocols";
        string targetAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;;
        string nomeCompletoDaClasse = $"{targetNamespace}.{ativo.type.ToUpperInvariant()}";
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
            return new Result(){ Message = $"The type {ativo.type.ToString()} was not implemented yet"};
        }

        try
        {
            MethodInfo metodo = type.GetMethod("Test");
            var result_task = metodo.Invoke(instance, new object[] { ativo });
            // Converte o retorno para o tipo esperado, por exemplo, string
            if (result_task is Task task)
            {
                await task; // Aguarda a conclusão da tarefa

                // Se o método retornar Task<T>, você pode acessar o resultado com reflection
                if (result_task.GetType().IsGenericType)
                {
                    var resultProperty = result_task.GetType().GetProperty("Result");
                    var result = resultProperty.GetValue(result_task);
                    return (Result) result;
                }
                else
                {
                    Console.WriteLine("Método assíncrono concluído.");
                    
                }
            }
        }
        catch (TargetInvocationException error)
        {
            return new Result(){ Message = error.Message};
        }
        catch (Exception error)
        {
            return new Result(){ Message = error.Message};
        }
        return new Result(){ Message = $"Something wrong was happend. Revisit the code on Program.cs"};
    }

}

