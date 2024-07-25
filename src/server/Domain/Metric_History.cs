using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SnowflakeID;

namespace Olimpo.Domain;

public class Metric_History
{
    [Key]
    public string id { get; set; } = SnowflakeIDGenerator.GetSnowflake(0).Id.ToString();

    public DateTime datetime { get; set; } = DateTime.UtcNow;


    public long latency { get; set; } = -1;
    public decimal? value { get; set; } = null;
}