namespace Olimpo.Domain;

public class Stack // like directory
{
    public Guid id { get; set; } = Guid.NewGuid();

    public string name { get; set; }

    public int order { get; set; }


    public List<Service>? services { get; set; }
}
