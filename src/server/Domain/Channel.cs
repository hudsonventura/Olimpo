namespace Olimpo.Domain;

public class Channel
{
    public Guid id { get; set; }

    public int channel_id { get; set; } = 0;

    public string name { get; set; }

    public Alert? alerts { get; set; }

    public Metric metric { get; set; } = new Metric();
}
