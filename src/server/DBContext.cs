using Microsoft.EntityFrameworkCore;
using Olimpo.Domain;
using Olimpo.Sensors;

namespace Olimpo;

public class Context : DbContext
{
    private string stringConnection = $@"Value not changed yet";
    public Context(IConfiguration appsettings) : base()
    {
        stringConnection = appsettings.GetConnectionString("DefaultConnection");
        //Console.WriteLine(_dbStringConnection);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(stringConnection);




    public DbSet<Alert> alerts { get; set; }

    public DbSet<Metric> metrics { get; set; }

    public DbSet<Channel> channels { get; set; }

    public DbSet<Sensor> sensors { get; set; }
}
