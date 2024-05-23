using Microsoft.EntityFrameworkCore;
using Olimpo.Domain;

namespace Olimpo;

public class Context : DbContext
{
    private string stringConnection = $@"valor ainda nao preenchido";
    public Context(IConfiguration appsettings) : base()
    {
        stringConnection = appsettings.GetConnectionString("DefaultConnection");
        //Console.WriteLine(_dbStringConnection);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(stringConnection);




    public DbSet<Alert> alerts { get; set; }
}
