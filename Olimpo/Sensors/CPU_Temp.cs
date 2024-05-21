using Olimpo.Domain;

namespace Olimpo.Sensors;

public class CPU_TEMP : ISensor
{
    public string GetUnit()
    {
        return "ºC";
    }

    public async Task<Result> Test(Service service, Sensor sensor)
    {
        //read info

        return new Result(){
            Latency  = 0,
            Value = 37,
            Message = "Got ok"
        };
    }
}
