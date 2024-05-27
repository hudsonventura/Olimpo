using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public interface ISensor
{
    public Task<List<Channel>> Test(Service service, Sensor sensor);
}
