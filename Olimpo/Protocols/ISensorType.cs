using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Protocols;

public interface ISensorType
{
    public Task<Result> Test(Service service, Sensor sensor);

    public string GetUnit();
}
