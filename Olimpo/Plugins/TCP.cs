using System.Net.Sockets;
using Olimpo;

namespace Olimpo.Plugins;

public class TCP
{
    public async Task<long> Test(Ativo ativo){
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            using (TcpClient client = new TcpClient())
            {
                var task = client.ConnectAsync(ativo.host, ativo.port);
                var result = await Task.WhenAny(task, Task.Delay(1000)); // Timeout de 5 segundos
                stopwatch.Stop();
                bool isPortOpen = result == task && client.Connected;
                long portLatency = stopwatch.ElapsedMilliseconds;
                return (isPortOpen == true) ? portLatency : -1;
            }
        }
        catch
        {
            stopwatch.Stop();
            return -1;
        }
    }
}
