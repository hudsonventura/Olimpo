using System.Security.Principal;

namespace Olimpo.Domain;

public class Sensor
{
    public Guid id { get; set; }

    public string name { get; set; }


    public string type { get; set; }


    public int port { get; set; }
    public int timeout { get; set; } // in milliseconds

    public string username { get; set; }
    public string password { get; set; }

    public int check_each { get; set; } = 1000; //re-check the sensor each a num of milliseconds. Default is 1000 seconds

    
    public Alert alerts { get; set; }


    public List<Metric> metrics { get; set; } = new List<Metric>();
}
