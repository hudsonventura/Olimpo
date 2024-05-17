using System.Reflection;
using Microsoft.Extensions.Configuration;
using Olimpo;
using Olimpo.Plugins;
using Spectre.Console;

public class Program
{
    static IConfiguration appsettings;
    static List<Ativo> ativos;

    static (Ativo, Result)[] results;

    static Table table = new Table().Centered();

    public static async Task Main(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .Build();

 

        table = new Table().Centered();
        table.Title("[yellow]Título da Tabela[/]");
        table.AddColumn("Name");
        table.AddColumn("Host");
        table.AddColumn("Protocol");
        table.AddColumn("Port");
        table.AddColumn("Latency");
        table.AddColumn("Status");


        
        while(true){
            Task taskData = Task.Run(() => ObtainValues());
            Task taskTable = Task.Run(() => ShowTable());
            await Task.Delay(1000);


        }

    }






    static async void ObtainValues(){
        ativos = appsettings.GetSection("ativos").Get<List<Ativo>>();
        results = new (Ativo, Result)[ativos.Count];
        for (int i = 0; i < ativos.Count; i++)
        {
            var result = await CheckHost(ativos[i]);
            results[i] = (ativos[i], result);
        }
    }

    static async void ShowTable(){
        if(results == null || results.Length == 0){
            return;
        }

 
        var results_internal = results;

        
        table.Rows.Clear();
        foreach (var (ativo, result) in results_internal)
        {
            if(ativo == null || result == null){
                continue;
            }
            var color = (result.Latency == -1) ? "red" : result.Latency < 50 ? "green" : result.Latency < 100 ? "yellow" : "red";
            var value = (result.Latency == -1) ? "-" : result.Latency.ToString()+"ms";

            table.AddRow(ativo.name, ativo.host, ativo.type.ToString(), ativo.port.ToString(), $"[{color}]{value}[/]", $"[{color}]{result.Message}[/]");
        }
        try
        {
            
            AnsiConsole.Clear();
        }
        catch (System.Exception)
        {
            
        }
        AnsiConsole.Write(table);

    }

    static async Task<Result> CheckHost(Ativo ativo)
    {
        string targetNamespace = "Olimpo.Plugins";
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
            return new Result(){ Latency = -1, Message = $"The type {ativo.type.ToString()} was not implemented yet"};
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
            return new Result(){ Latency = -1, Message = error.Message};
        }
        catch (Exception error)
        {
            return new Result(){ Latency = -1, Message = error.Message};
        }
        return new Result(){ Latency = -1, Message = $"Something wrong was happend. Revisit the code on Program.cs"};
    }

}

