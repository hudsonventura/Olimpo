using Microsoft.EntityFrameworkCore;
using Olimpo.Domain;
using Olimpo.Sensors;

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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





    public DbSet<Stack> stacks { get; set; }

        public DbSet<Service> services { get; set; }

            public DbSet<Sensor> sensors { get; set; }

                public DbSet<Channel> channels { get; set; }
    
                    public DbSet<Alert> alerts { get; set; }

                    public DbSet<Metric> metrics { get; set; }

    

    private static readonly ConcurrentQueue<Func<Task>> SaveQueue = new ConcurrentQueue<Func<Task>>();
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

    public async static Task EnqueueOperation(Func<Task> saveOperation)
    {
        SaveQueue.Enqueue(saveOperation);
        _ = ProcessSaveQueueAsync();  // Chama o processamento da fila, ignorando a Task retornada
    }

    private static async Task ProcessSaveQueueAsync()
    {
        await Semaphore.WaitAsync();
        try
        {
            while (SaveQueue.TryDequeue(out var saveOperation))
            {
                try
                {
                    await saveOperation();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar a operação de salvamento: {ex.Message}");
                }
            }
        }
        finally
        {
            Semaphore.Release();
        }
    }
    
}
