using System.Net.Sockets;
using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Protocols;

public class TCP : ISensorType
{

    public async Task<Result> Test(Service service, Sensor sensor)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            using (TcpClient client = new TcpClient())
            {
                var connectTask = client.ConnectAsync(service.host, sensor.port);
                var timeoutTask = Task.Delay(sensor.timeout); // Timeout of 5 seconds
                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    return new Result() { Message = $"The port {sensor.port} is not opened or a timeout was got"};
                }

                var isPortOpen = client.Connected;
                if (!isPortOpen)
                {
                    return new Result() { Message = $"The port {sensor.port} is not opened" };
                }
                return new Result() { Message = "Success", Latency = stopwatch.ElapsedMilliseconds , Value = stopwatch.ElapsedMilliseconds };
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
