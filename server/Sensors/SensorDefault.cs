using Olimpo.Domain;

namespace Olimpo.Sensors;

public class SensorGenDefaultChannel
{
    public List<Channel> GenChannels(Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();
        channels.Add(new Channel(){
            name = sensor.name,
            channel_id = 1,
        });
        return channels;
    }
}
