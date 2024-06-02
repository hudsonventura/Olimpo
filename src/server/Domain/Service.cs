using Olimpo.Sensors;

namespace Olimpo.Domain;

public class Service  // like a file
{
    public Guid id { get; set; }

    public string name { get; set; }


    public string host { get; set; }
    

    public List<Sensor> sensors { get; set; }
}
