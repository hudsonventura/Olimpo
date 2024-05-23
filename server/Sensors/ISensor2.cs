using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public interface ISensor2
{
    public Task<Sensor> Test(Service service, Sensor sensor);

    public List<Channel> GenChannels(Sensor sensor);
}
