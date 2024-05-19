namespace Olimpo.Domain;

public class Sensor
{
    public Guid id { get; set; }

    public string name { get; set; }


    public string type { get; set; }


    public int port { get; set; }
    public int timeout { get; set; } // in milliseconds


    public List<Metric> metrics { get; set; } = new List<Metric>();
}
