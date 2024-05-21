using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public interface ISensor
{
    public Task<Result> Test(Service service, Sensor sensor);

    public string GetUnit();
}
