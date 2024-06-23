using Olimpo;
using Olimpo.Domain;

namespace Olimpo.Sensors;

public interface ISensor
{
    /// <summary>
    /// Do the test of device/channels
    /// </summary>
    /// <param name="service"></param>
    /// <param name="sensor"></param>
    /// <returns></returns>
    public Task<List<Channel>> Test(Device service, Sensor sensor);

    /// <summary>
    /// Inform the type name to show at frontend side
    /// </summary>
    /// <returns></returns>
    public string GetType();
}
