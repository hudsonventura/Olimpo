namespace Olimpo.Domain;

public class Service
{
    public Guid id { get; set; }

    public string name { get; set; }


    public string host { get; set; }
    

    public List<Sensor> sensors { get; set; }
}
