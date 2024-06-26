namespace Olimpo.Domain;

public class Channel
{
    public Guid id { get; set; }
    public string name { get; set; }


    public int channel_id { get; set; } = 0;

    

    public Alert? alerts { get; set; }


    public string? unit { get; set; }

    public Metric current_metric { get; set; } = new Metric();
    public List<Metric> metrics { get; set; } = new List<Metric>();
}
