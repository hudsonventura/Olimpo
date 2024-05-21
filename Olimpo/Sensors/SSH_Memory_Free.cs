using System.Diagnostics;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSH_MEMORY_FREE : ISensor
{
    public string GetUnit()
    {
        return "GB";
    }

    public async Task<Result> Test(Service service, Sensor sensor)
    {
        string command = @"free -k | awk '/Mem:/ {printf "" %.2f"", $7/1024/1024}'";

        using (var client = new SshClient(service.host, sensor.username, sensor.password))
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                client.Connect();

                if (client.IsConnected)
                {
                    var commandResult = client.RunCommand(command);

                    string numberWithoutComma = commandResult.Result.Replace(".", ",");

                    return new Result(){
                        Latency = stopwatch.ElapsedMilliseconds,
                        Message = "Ok",
                        Value = decimal.Parse(numberWithoutComma)
                    };
                }
                else
                {
                    Console.WriteLine("Não foi possível conectar ao servidor SSH.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                client.Disconnect();
            }
        }
        return new Result(){
                        Message = "SSH was not executed"
                    };
    }
}
