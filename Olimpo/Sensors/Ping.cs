using Olimpo.Domain;
namespace Olimpo.Sensors;

public class PING : SensorGenDefaultChannel, ISensor2
{
    public async Task<Sensor> Test(Service service, Sensor sensor)
    {
        var ping = new System.Net.NetworkInformation.Ping();

        Metric result = null;

        System.Net.NetworkInformation.PingReply pingReply;
        try
        {
            using (var cts = new CancellationTokenSource())
            {
                cts.CancelAfter(TimeSpan.FromMilliseconds(sensor.timeout)); // Timeout de 5 segundos
                var task = ping.SendPingAsync(service.host);
                var completedTask = await Task.WhenAny(task, Task.Delay(Timeout.Infinite, cts.Token));

                if (completedTask == task)
                {
                    // A operação de ping completou dentro do tempo limite
                    pingReply = await task;
                    bool isPingSuccess = pingReply.Status == System.Net.NetworkInformation.IPStatus.Success;
                    result = new Metric() { message = "Success", latency = pingReply.RoundtripTime, value = pingReply.RoundtripTime };
                }
                else
                {
                    // A operação de ping foi cancelada devido ao timeout
                    result = new Metric() { message = "Ping timeout" };
                }
            }
        }
        catch (System.Exception error)
        {
            result = new Metric() { message = error.Message };
        }
        sensor.channels[0].metric = result;
        return sensor;
    }
}
