using Olimpo.Domain;

namespace Olimpo.Sensors;

public class CPU : ISensor
{
    public async Task<List<Channel>> Test(Device service, Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();

        Random rand = new Random();

        channels.Add(new Channel(){
                channel_id = 1,
                name = $"{sensor.name} - % usage CPU",
                unit = "%",
                current_metric = new Metric(){
                    latency  = 0,
                    value = rand.Next(0, 100),
                    message = "Success",
                    status = Metric.Status.Success
                }
        });

        channels.Add(new Channel(){
                channel_id = 2,
                name = $"{sensor.name} - Temp",
                unit = "ºC",
                current_metric = new Metric(){
                    latency  = 0,
                    value = 39,
                    message = "Success",
                    status = Metric.Status.Success
                }
        });

        return channels;
    }
}
