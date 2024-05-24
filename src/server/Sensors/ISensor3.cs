using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public interface ISensor3
{
    public Task<List<Channel>> Test(Service service, Sensor sensor);
}
