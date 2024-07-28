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


    public (Metric.Status, string) DetermineStatus()
    {

        if(current_metric.status == Metric.Status.Offline){
            return (Metric.Status.Offline, this.current_metric.message);
        }

        if(upper_error is not null && current_metric.value >= upper_error){
            return (Metric.Status.Error, $"Value {current_metric.value} is greater than {upper_error}{unit}");
        }

        if(upper_warning is not null && current_metric.value >= upper_warning){
            return (Metric.Status.Warning, $"Value {current_metric.value} is greater than {upper_warning}{unit}");
        }

        if(lower_error is not null && current_metric.value <= lower_error){
            return (Metric.Status.Error, $"Value {current_metric.value} is lower than {lower_error}{unit}");
        }

        if(lower_warning is not null && current_metric.value <= lower_warning){
            return (Metric.Status.Warning, $"Value {current_metric.value} is lower than {lower_error}{unit}");
        }

        return (current_metric.status, current_metric.message);
    }
}
