using System.Net.Sockets;
using Olimpo;

namespace Olimpo.Plugins;

public class TCP : IPlugin
{

    public async Task<Result> Test(Ativo ativo)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            using (TcpClient client = new TcpClient())
            {
                var connectTask = client.ConnectAsync(ativo.host, ativo.port);
                var timeoutTask = Task.Delay(ativo.timeout); // Timeout of 5 seconds
                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    return new Result() { Message = $"The port {ativo.port} is not opened or a timeout was got"};
                }

                var isPortOpen = client.Connected;
                if (!isPortOpen)
                {
                    return new Result() { Message = $"The port {ativo.port} is not opened" };
                }
                return new Result() { Message = "Success", Latency = stopwatch.ElapsedMilliseconds };
            }
        }
        catch(Exception error)
        {
            return new Result(){ Message = error.Message};
        }
        finally{
            stopwatch.Stop();
        }
    }
}
