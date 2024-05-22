using Olimpo;
using Olimpo.Domain;
using Microsoft.Extensions.Configuration;

public class Program
{
    static List<Stack> stacks;
    public static async Task Main(string[] args)
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        IConfiguration appsettings = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .Build();

        stacks = appsettings.GetSection("stacks").Get<List<Stack>>();


        //star threads to check each sensor
        SensorsChecker.StartLoopChecker(stacks);

        while(true){
            ConsoleExhibitor.Show(stacks);
            Thread.Sleep(3000);
        }
    }
}