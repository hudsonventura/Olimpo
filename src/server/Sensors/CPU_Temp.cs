using Olimpo.Domain;

namespace Olimpo.Sensors;

public class CPU_TEMP : ISensor
{
    public List<Channel> GenChannels(Sensor sensor)
    {
        return null;
    }

    public string GetUnit()
    {
        return "ºC";
    }

    public async Task<Metric> Test(Service service, Sensor sensor)
    {
        //read info

        return new Metric(){
            latency  = 0,
            value = 37,
            message = "Got ok"
        };
    }
}
