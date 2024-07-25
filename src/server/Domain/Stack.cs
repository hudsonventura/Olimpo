using System.ComponentModel.DataAnnotations;
using SnowflakeID;

namespace Olimpo.Domain;

public class Stack // like directory
{
    [Key]
    public string id { get; set; } = SnowflakeIDGenerator.GetSnowflake(0).Id.ToString();

    public string name { get; set; }

    public int order { get; set; }


    public List<Device>? devices { get; set; }
}
