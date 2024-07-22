using Microsoft.EntityFrameworkCore;
using Olimpo.Domain;
using Olimpo.Sensors;

using System.Collections.Concurrent;

namespace Olimpo;

public class Context : DbContext
{
    private string stringConnection = $@"Value not changed yet";
    public Context(IConfiguration appsettings) : base()
    {
        stringConnection = appsettings.GetConnectionString("DefaultConnection");
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(stringConnection);





    public DbSet<Stack> stacks { get; set; }

        public DbSet<Device> devices { get; set; }

            public DbSet<Sensor> sensors { get; set; }

                public DbSet<Channel> channels { get; set; }
    

                    public DbSet<Metric> metrics { get; set; }

    



    
}
