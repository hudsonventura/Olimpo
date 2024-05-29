using System.Net.Sockets;
using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public class TCP : ISensor
{
    
    public async Task<List<Channel>> Test(Service service, Sensor sensor)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        Metric metric = null;
        try
        {
            using (TcpClient client = new TcpClient())
            {
                string host = (service.host != null) ? service.host : service.host;

                var connectTask = client.ConnectAsync(service.host, (int)sensor.port);
                var timeoutTask = Task.Delay(sensor.timeout); // Timeout of 5 seconds
                var completedTask = await Task.WhenAny(connectTask, timeoutTask);
                if (completedTask == timeoutTask)
                {
                    metric = new Metric() { 
                        message = $"The port {sensor.port} is not opened or a timeout was got",
                        latency = stopwatch.ElapsedMilliseconds,
                        error_code = 1
                    };
                }

                var isPortOpen = client.Connected;
                if (isPortOpen)
                {
                    metric = new Metric() { 
                        message = "Success", 
                        latency = stopwatch.ElapsedMilliseconds, 
                        value = stopwatch.ElapsedMilliseconds 
                    };
                }
                else
                {
                    metric = new Metric() { 
                        message = $"The port {sensor.port} is not opened or a timeout was got",
                        latency = stopwatch.ElapsedMilliseconds,
                        error_code = 1
                    };
                }
            }
        }
        catch(Exception error)
        {
            metric = new Metric(){ 
                message = error.Message,
                latency = stopwatch.ElapsedMilliseconds,
                error_code = 1
            };
        }
        finally{
            stopwatch.Stop();
        }

        List<Channel> channels = new List<Channel>();
        channels.Add(
            new Channel(){
                name = $"{sensor.name} - Generic TCP",
                channel_id = 1,
                current_metric = metric,
                unit = "ms"
            }
        );
        return channels;
    }
}
