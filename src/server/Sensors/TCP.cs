using System.Net.Sockets;
using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public class TCP : ISensor
{

    public List<Channel> GenChannels(Sensor sensor)
    {
        return null;
    }

    public string GetUnit()
    {
        return "ms";
    }
    
    public async Task<Metric> Test(Service service, Sensor sensor)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            using (TcpClient client = new TcpClient())
            {
                string host = (service.host != null) ? service.host : "localhost";

                var connectTask = client.ConnectAsync(service.host, (int)sensor.port);
                var timeoutTask = Task.Delay(sensor.timeout); // Timeout of 5 seconds
                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    return new Metric() { message = $"The port {sensor.port} is not opened or a timeout was got"};
                }

                var isPortOpen = client.Connected;
                if (!isPortOpen)
                {
                    return new Metric() { message = $"The port {sensor.port} is not opened" };
                }
                return new Metric() { message = "Success", latency = stopwatch.ElapsedMilliseconds , value = stopwatch.ElapsedMilliseconds };
            }
        }
        catch(Exception error)
        {
            return new Metric(){ message = error.Message};
        }
        finally{
            stopwatch.Stop();
        }
    }
}
