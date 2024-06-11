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
