namespace Olimpo.Domain;

public abstract class Hierarquical
{
    public Guid id { get; set; }

    public string name { get; set; }
    public int order { get; set; }
    public Stack? up { get; set; }
}
