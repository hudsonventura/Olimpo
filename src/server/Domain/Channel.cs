using System.ComponentModel.DataAnnotations;
using SnowflakeID;

namespace Olimpo.Domain;

public class Channel
{
    [Key]
    public string id { get; set; } = SnowflakeIDGenerator.GetSnowflake(0).Id.ToString();
    public string name { get; set; }


    public int channel_id { get; set; } = 0;

    


    public string? unit { get; set; }


    public decimal? upper_error { get; set; }

    public decimal? upper_warning { get; set; }
    
    public decimal? lower_warning { get; set; }
    public decimal? lower_error { get; set; }


    public Metric current_metric { get; set; }
    public List<Metric_History> metrics { get; set; } = new List<Metric_History>();

}
