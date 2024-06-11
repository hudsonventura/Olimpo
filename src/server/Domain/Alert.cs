using System.ComponentModel.DataAnnotations;

namespace Olimpo.Domain;

public class Alert
{
    [Key]
    public 	Guid id { get; set; }
    public Type type { get; set; }
    public int critical { get; set; }
    public int warning { get; set; }
    public int success { get; set; }

    public enum Type{
        lower_better,
        upper_better,
        exact
    }
}
