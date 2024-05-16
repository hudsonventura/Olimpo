using System;

using System.Collections.Generic;

using System.Net.NetworkInformation;

using System.Net.Sockets;

using System.Threading.Tasks;

using Spectre.Console;




        var console = AnsiConsole.Console;

        var table = new Table().Centered();

        table.AddColumn("Host");

        table.AddColumn("Port");

        table.AddColumn("Ping");

        table.AddColumn("Port Status");

        table.AddColumn("Latency");



        await AnsiConsole.Live(table)

            .AutoClear(false) // Não remover quando terminar

            .StartAsync(async ctx =>

            {

                while (true)

                {

                    var results = new List<(string host, int port, bool isPortOpen, long pingTime, long portLatency)>();



                    await CheckHosts(results, new[] { "10.10.50.50", "10.10.50.51", "10.10.50.52", "10.100.100.43" }, 53);

                    await CheckHosts(results, new[] { "10.100.51.36", "10.100.50.100" }, 3200);

                    await CheckHosts(results, new[] { "10.100.2.117", "10.100.2.61", "10.100.2.22", "10.100.2.220", "10.100.2.136", "10.100.2.253" }, 6244);



                    table.Rows.Clear();

                    foreach (var (host, port, isPortOpen, pingTime, portLatency) in results)

                    {

                        DisplayResult(table, host, port, isPortOpen, pingTime, portLatency);

                    }



                    ctx.Refresh();

                    await Task.Delay(1000); // Atualiza a cada segundo

                }

            });

    



    static async Task CheckHosts(List<(string, int, bool, long, long)> results, string[] hosts, int port)

    {

        foreach (var host in hosts)

        {

            var result = await CheckHost(host, port);

            results.Add(result);

        }

    }



    static async Task<(string, int, bool, long, long)> CheckHost(string host, int port)

    {

        var ping = new Ping();

        var pingReply = await ping.SendPingAsync(host);

        bool isPingSuccess = pingReply.Status == IPStatus.Success;

        long pingTime = pingReply.RoundtripTime;



        var (isPortOpen, portLatency) = await CheckPortAsync(host, port);



        return (host, port, isPingSuccess && pingReply.Status == IPStatus.Success, pingTime, portLatency);

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

                return (isPortOpen, portLatency);

            }

        }

        catch

        {

            stopwatch.Stop();

            return (false, stopwatch.ElapsedMilliseconds);

        }

    }



    static void DisplayResult(Table table, string host, int port, bool isPortOpen, long pingTime, long portLatency)

    {

        var pingColor = pingTime < 100 ? "green" : pingTime < 300 ? "yellow" : "red";

        var portColor = isPortOpen ? "green" : "red";



        table.AddRow(host, port.ToString(), $"{pingTime}ms", isPortOpen ? "Open" : "Closed", $"{portLatency}ms");

    }

