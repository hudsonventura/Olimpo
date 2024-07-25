using System.ComponentModel.DataAnnotations;
using Olimpo.Sensors;
using SnowflakeID;

namespace Olimpo.Domain;

public class Device
{
    [Key]
    public string id { get; set; } = SnowflakeIDGenerator.GetSnowflake(0).Id.ToString();

    public string name { get; set; }


    public string host { get; set; }
    

    public List<Sensor>? sensors { get; set; }
}
