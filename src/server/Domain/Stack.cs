namespace Olimpo.Domain;

public class Stack : Hierarquical // like directory
{
    public Guid id { get; set; }

    public string name { get; set; }


    public List<Service> services { get; set; }
}
