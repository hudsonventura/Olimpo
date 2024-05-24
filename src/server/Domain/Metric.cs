namespace Olimpo.Domain;

public class Metric
{
    public Guid id { get; set; }

    public DateTime datetime { get; set; } = DateTime.UtcNow;


    public long latency { get; set; } = -1; // how many time it spends to obtain the metric value. It's cool for ping, tcp, query on db, etc.
    public decimal value { get; set; } = -1; //-1 is default value informing that it was not changed
    public string message { get; set; } = "Message was not set yet";

    public int error_code { get; set; } = 0; //0 inform no error

    
}
