using System.Net.NetworkInformation;

using System.Net.Sockets;

using Microsoft.Extensions.Configuration;
using Olimpo;
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

    table.AddColumn("Nome");

    table.AddColumn("IP");

    table.AddColumn("Porta");

    table.AddColumn("Ping");

    table.AddColumn("Porta Status");

    table.AddColumn("Latencia");


    await AnsiConsole.Live(table)

        .AutoClear(false) // Não remover quando terminar

        .StartAsync(async ctx =>

        {

            while (true)

            {

                var results = new List<(Ativo ativo, bool isPortOpen, long pingTime, long portLatency)>();

                var ativos = appsettings.GetSection("ativos").Get<List<Ativo>>();
                
                foreach (var ativo in ativos)
                {

                    var result = await CheckHost(ativo);

                    results.Add(result);

                }



                table.Rows.Clear();

                foreach (var (ativo, isPortOpen, pingTime, portLatency) in results)

                {

                    DisplayResult(table, ativo, isPortOpen, pingTime, portLatency);

                }



                ctx.Refresh();

                await Task.Delay(1000); // Atualiza a cada segundo

            }

        });







static async Task<(Ativo, bool, long, long)> CheckHost(Ativo ativo)

{

    var ping = new Ping();

    PingReply pingReply;
    bool isPingSuccess = false;
    long pingTime = -1;
    try
    {
        pingReply = await ping.SendPingAsync(ativo.ip);

        isPingSuccess = pingReply.Status == IPStatus.Success;

        pingTime = pingReply.RoundtripTime;
    }
    catch (System.Exception)
    {
        
    }
    



    var (isPortOpen, portLatency) = await CheckPortAsync(ativo.ip, ativo.porta);



    return (ativo, isPingSuccess, pingTime, portLatency);

}



static async Task<(bool, long)> CheckPortAsync(string host, int port)

{

    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

    try

    {

        using (TcpClient client = new TcpClient())

        {

            var task = client.ConnectAsync(host, port);

            var result = await Task.WhenAny(task, Task.Delay(1000)); // Timeout de 5 segundos

            stopwatch.Stop();

            bool isPortOpen = result == task && client.Connected;

            long portLatency = stopwatch.ElapsedMilliseconds;

            return (isPortOpen, (isPortOpen == true) ? portLatency : -1);

        }

    }

    catch

    {

        stopwatch.Stop();

        return (false, -1);

    }

}



static void DisplayResult(Table table, Ativo ativo, bool isPortOpen, long pingTime, long portLatency)

{
    var pingColor = (pingTime == -1) ? "red" : pingTime < 60 ? "green" : pingTime < 100 ? "yellow" : "red";

    var portColor = (portLatency == -1) ? "red" : portLatency < 60 ? "green" : portLatency < 100 ? "yellow" : "red";

    var portOpen = isPortOpen ? "Open" : "Closed";


    table.AddRow(ativo.nome, ativo.ip, ativo.porta.ToString(), $"[{pingColor}]{pingTime}ms[/]", $"[{portColor}]{portOpen}[/]", $"[{portColor}]{portLatency}ms[/]");
}

