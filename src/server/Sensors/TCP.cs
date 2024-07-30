using System.Net.Sockets;
using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public class TCP : ISensor
{
    public string GetType()
    {
        return "Generic TCP (like Telnet trying connect)";
    }


    public async Task<List<Channel>> Test(Device service, Sensor sensor)
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
                        value = sensor.timeout,
                        status = Metric.Status.Error
                    };
                }

                var isPortOpen = client.Connected;
                if (isPortOpen)
                {
                    metric = new Metric() { 
                        message = "Success", 
                        latency = stopwatch.ElapsedMilliseconds, 
                        value = stopwatch.ElapsedMilliseconds,
                        status = Metric.Status.Success
                    };
                }
                else
                {
                    metric = new Metric() { 
                        message = $"The port {sensor.port} is not opened or a timeout was got",
                        latency = stopwatch.ElapsedMilliseconds,
                        value = sensor.timeout,
                        status = Metric.Status.Error
                    };
                }
            }
        }
        catch(Exception error)
        {
            metric = new Metric(){ 
                message = error.Message,
                latency = stopwatch.ElapsedMilliseconds,
                status = Metric.Status.Error
            };
        }
        finally{
            stopwatch.Stop();
        }

        List<Channel> channels = new List<Channel>();

        if(sensor.channels.Count() == 0){
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

        var channel1 = sensor.channels[0];
        channel1.current_metric = metric;
        channels.Add(channel1);
        return channels;
        
    }
}
