using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public interface ISensor
{
    public Task<Metric> Test(Service service, Sensor sensor);

    public List<Channel> GenChannels(Sensor sensor);
}
