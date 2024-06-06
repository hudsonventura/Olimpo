using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public interface ISensor
{
    public Task<List<Channel>> Test(Device service, Sensor sensor);
}
