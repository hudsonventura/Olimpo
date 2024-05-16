using System.Net.NetworkInformation;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Olimpo;
using Olimpo.Plugins;
using Spectre.Console;



var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfiguration appsettings = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.AddJsonFile($"appsettings.{env}.json", optional: true)
	.Build();



var console = AnsiConsole.Console;
var table = new Table().Centered();
table.Title("[yellow]Título da Tabela[/]");
table.AddColumn("Name");
table.AddColumn("Host");
table.AddColumn("Protocol");
table.AddColumn("Port");
table.AddColumn("Latency");
table.AddColumn("Status");


        
while (true)
{
    var results = new List<(Ativo, long)>();
    var ativos = appsettings.GetSection("ativos").Get<List<Ativo>>();
    foreach (var ativo in ativos)
    {
        var result = await CheckHost(ativo);
        results.Add((ativo, result));
    }

    
    try
    {
        table.Rows.Clear();
        AnsiConsole.Clear();
    }
    catch (System.Exception)
    {
        
    }

    foreach (var (ativo, result) in results)
    {
        var color = (result == -1) ? "red" : result < 50 ? "green" : result < 100 ? "yellow" : "red";
        var status = (result == -1) ? "Error" : "Success";

        table.AddRow(ativo.name, ativo.host, ativo.type.ToString(), ativo.port.ToString(), $"[{color}]{result}ms[/]", $"[{color}]{status}[/]");
    }
    AnsiConsole.Write(table);
    await Task.Delay(1000); // Atualiza a cada segundo
}




static async Task<long> CheckHost(Ativo ativo)
{
    string targetNamespace = "Olimpo.Plugins";
    string targetAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;;
    string nomeCompletoDaClasse = $"{targetNamespace}.{ativo.type}";
    Assembly assembly = Assembly.Load(targetAssemblyName);


    var  fullClassName  = assembly.GetType(nomeCompletoDaClasse);
    Type type = fullClassName;
    object instance = Activator.CreateInstance(type);

    MethodInfo metodo = type.GetMethod("Test");
    if (metodo != null)
    {
        metodo.Invoke(null, null);
    }

    switch (ativo.type)
    {
        case Ativo.Type.Ping: return await TestPing(ativo.host);
        
        //case Ativo.Type.TCP: return await TestPort(ativo);

    
        default: throw new Exception($"The type {ativo.type.ToString()} was not implemented yet");
    }
}

static async Task<long> TestPing(string host){
    var ping = new Ping();

    PingReply pingReply;
    bool isPingSuccess = false;
    long pingTime = -1;
    try
    {
        pingReply = await ping.SendPingAsync(host);
        isPingSuccess = pingReply.Status == IPStatus.Success;
        return pingReply.RoundtripTime;
    }
    catch (System.Exception)
    {
        return -1;
    }
}





static void DisplayResult(Table table, Ativo ativo, long result)
{
    var color = (result == -1) ? "red" : result < 60 ? "green" : result < 100 ? "yellow" : "red";
    var status = (result == -1) ? "Error" : "Success";

    table.AddRow(ativo.name, ativo.host, ativo.type.ToString(), ativo.port.ToString(), $"[{color}]{result}ms[/]", $"[{color}]{status}[/]");
}