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


    public decimal success_value { get; set; }
    public Orientation success_orientation { get; set; }

    public decimal warning_value { get; set; }
    public Orientation warning_orientation { get; set; }
    
    public decimal danger_value { get; set; }
    public Orientation danger_orientation { get; set; }


    public Metric current_metric { get; set; }
    public List<Metric_History> metrics { get; set; } = new List<Metric_History>();


    


    public enum Orientation {
        Disabled = 0,
        GreaterThan = 1,
        GreaterThanOrEqual = 2,
        Equal = 3,
        LessThanOrEqual = 4,
        LessThan = 5
    }
}
