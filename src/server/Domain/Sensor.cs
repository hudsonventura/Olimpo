using System.ComponentModel.DataAnnotations;
using Olimpo.Domain;
using SnowflakeID;

namespace Olimpo.Sensors;

public partial class Sensor
{
    [Key]
    public string id { get; set; } = SnowflakeIDGenerator.GetSnowflake(0).Id.ToString();

    public string name { get; set; }


    public string type { get; set; }
    public int check_each { get; set; } = 1000; //re-check the sensor each a num of milliseconds. Default is 1000 seconds


    public string? host { get; set; } //specific host that will override the service host (in case of different endpoint)
    public int? port { get; set; }
    public int timeout { get; set; } // in milliseconds

    public string? username { get; set; }
    public string? password { get; set; }

    

    public List<Channel>? channels { get; set; } = new List<Channel>();

    
}
