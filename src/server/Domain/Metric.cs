using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Olimpo.Domain;

public class Metric
{
    public Guid id { get; set; }

    public DateTime datetime { get; set; } = DateTime.UtcNow;


    public long latency { get; set; } = -1; // how many time it spends to obtain the metric value. It's cool for ping, tcp, query on db, etc.
    public decimal? value { get; set; } = null; //-1 is default value informing that it was not changed
    public string message { get; set; } = "Message was not set yet";

    [JsonConverter(typeof(StringEnumConverter))]
    public Status status { get; set; } = Status.NotChecked;

    

    public enum Status{
        NotChecked = 0,
        Success = 1,
        Warning = 2,
        Paused = 3,
        Error = -1

    }
}
