using System.Diagnostics;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSH_MEMORY_USED : ISensor
{
    public List<Channel> GenChannels(Sensor sensor)
    {
        return null;
    }

    public string GetUnit()
    {
        return "GB";
    }

    public async Task<Metric> Test(Service service, Sensor sensor)
    {
        string command = @"free -k | awk '/Mem:/ {printf "" %.2f"", $3/1024/1024}'";

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

                    return new Metric(){
                        latency = stopwatch.ElapsedMilliseconds,
                        message = "Ok",
                        value = decimal.Parse(numberWithoutComma)
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
        return new Metric(){ message = "SSH was not executed" };
    }
}
