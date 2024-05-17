
namespace Olimpo.Plugins;

public class PING : IPlugin
{
    public async Task<Result> Test(Ativo ativo)
    {
        var ping = new System.Net.NetworkInformation.Ping();

        System.Net.NetworkInformation.PingReply pingReply;
        try
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromMilliseconds(ativo.timeout)); // Timeout de 5 segundos
                var task = ping.SendPingAsync(ativo.host);
                var completedTask = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cts.Token));

                if (completedTask == task)
                {
                    // A operação de ping completou dentro do tempo limite
                    pingReply = await task;
                    bool isPingSuccess = pingReply.Status == System.Net.NetworkInformation.IPStatus.Success;
                    return new Result() { Message = "Success", Latency = pingReply.RoundtripTime };
                }
                else
                {
                    // A operação de ping foi cancelada devido ao timeout
                    return new Result() { Message = "Ping timeout" };
                }
            }
        }
        catch (System.Exception error)
        {
            return new Result() { Message = error.Message };
        }
    }
}
