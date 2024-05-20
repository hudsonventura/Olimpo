namespace Olimpo.Domain;

public class Metric
{
    public Guid id { get; set; }

    public DateTime datetime { get; set; }


    public long latency { get; set; } // how many time cost to obtain the metric value. It's cool for ping, tcp, query on db, etc.
    public decimal value { get; set; } = -1; //-1 is default value informing that it was not changed
    public string unit { get; set; } = String.Empty; // a string to inform the unit of measurement  of value prop
    public int error_code { get; set; } = 0; //0 inform no error

    public string message { get; set; } = "Not message was set";
}
