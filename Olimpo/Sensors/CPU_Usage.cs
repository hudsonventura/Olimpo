using Olimpo.Domain;

namespace Olimpo.Sensors;

public class CPU_USAGE : ISensor
{
    public List<Channel> GenChannels(Sensor sensor)
    {
        return null;
    }

    public string GetUnit()
    {
        return "%";
    }

    public async Task<Metric> Test(Service service, Sensor sensor)
    {
        //read info
        Random rand = new Random();
        return new Metric(){
            latency  = 0,
            value = rand.Next(0, 100),
            message = "Got ok"
        };
    }
}
