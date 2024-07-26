using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SnowflakeID;

namespace Olimpo.Domain;

public class Metric
{
    [Key]
    public string id { get; set; } = SnowflakeIDGenerator.GetSnowflake(0).Id.ToString();

    public DateTime datetime { get; set; } = DateTime.UtcNow;


    public long latency { get; set; } = -1;
    public decimal? value { get; set; } = null;
    
    public string message { get; set; } = "Message was not set yet";

    [JsonConverter(typeof(StringEnumConverter))]
    public Status status { get; set; } = Status.NotChecked;

    public enum Status{
        NotChecked = 0,
        Success = 1,
        Warning = 2,
        Paused = 3,
        Error = -1, 
        Offline = -2

    }


    public Metric_History ToHistoricMetric(){
        return new Metric_History(){
            id = this.id,
            datetime = this.datetime,
            latency = this.latency,
            value = this.value
        };
    }
}
