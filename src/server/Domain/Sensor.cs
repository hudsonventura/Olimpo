using Olimpo.Domain;

namespace Olimpo.Sensors;

public partial class Sensor
{
    public Guid id { get; set; }

    public string name { get; set; }


    public string type { get; set; }

    public int channel { get; set; } = 1;
    public int port { get; set; }
    public int timeout { get; set; } // in milliseconds

    public string username { get; set; }
    public string password { get; set; }

    public int check_each { get; set; } = 1000; //re-check the sensor each a num of milliseconds. Default is 1000 seconds

    

    public List<Channel> channels { get; set; } = new List<Channel>();

    public MetricUnit metric_unit { get; set; } = MetricUnit.None;

    public enum MetricUnit{
        None,
        B, KB, MB, GB, TB,
        Bps, KBps, MBps, GBps, TBps,


        bi, Kbi, Mbi, Gbi, Tbi,
        bips, Kbips, Mbips, Gbips, Tbips, 


        Hz, KHz, MHz, GHz,

        ms, s, m, h, d,
        Days
    }

    public static decimal NumberToCorrectUnit(decimal value, MetricUnit unit){
        switch (unit)
        {
            case MetricUnit.None: return value;
            case MetricUnit.Days: return value;
            
            // Bits
            case MetricUnit.bi: return value;
            case MetricUnit.Kbi: return value / 1024;
            case MetricUnit.Mbi: return value / (1024 * 1024);
            case MetricUnit.Gbi: return value / (1024 * 1024 * 1024);
            case MetricUnit.Tbi: return value / (1024m * 1024 * 1024 * 1024);

            // Bits per second
            case MetricUnit.bips: return value;
            case MetricUnit.Kbips: return value / 1024;
            case MetricUnit.Mbips: return value / (1024 * 1024);
            case MetricUnit.Gbips: return value / (1024 * 1024 * 1024);
            case MetricUnit.Tbips: return value / (1024m * 1024 * 1024 * 1024);

            // Bytes
            case MetricUnit.B: return value;
            case MetricUnit.KB: return value / 1024;
            case MetricUnit.MB: return value / (1024 * 1024);
            case MetricUnit.GB: return value / (1024 * 1024 * 1024);
            case MetricUnit.TB: return value / (1024m * 1024 * 1024 * 1024);

            // Bytes per second
            case MetricUnit.Bps: return value;
            case MetricUnit.KBps: return value / 1024;
            case MetricUnit.MBps: return value / (1024 * 1024);
            case MetricUnit.GBps: return value / (1024 * 1024 * 1024);
            case MetricUnit.TBps: return value / (1024m * 1024 * 1024 * 1024);

            // Hertz
            case MetricUnit.Hz: return value;
            case MetricUnit.KHz: return value / 1000;
            case MetricUnit.MHz: return value / (1000 * 1000);
            case MetricUnit.GHz: return value / (1000 * 1000 * 1000);

            default: return value;
        }
    }
}
