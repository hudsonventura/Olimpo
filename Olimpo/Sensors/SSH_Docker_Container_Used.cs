using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSH_DOCKER_CONTAINER_USED : ISensor
{
    string unit = "";
    public string GetUnit()
    {
        return unit;
    }

    public async Task<Result> Test(Service service, Sensor sensor)
    {
        string command = $"sudo -S docker stats torrent --no-stream --format \"{{{{.MemUsage}}}}\"";

        var connectionInfo = new ConnectionInfo(service.host, sensor.username,
                new PasswordAuthenticationMethod(sensor.username, sensor.password));

        using (var client = new SshClient(connectionInfo))
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                client.Connect();

                if (client.IsConnected)
                {
                    ShellStream stream = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);
                     // Get logged in and get user prompt
                    string prompt = stream.Expect(new Regex(@"[$>]"));
                    //Console.WriteLine(prompt);

                    // Send command and expect password or user prompt
                    stream.WriteLine(command);
                    prompt = stream.Expect(new Regex(@"([$#>:])"));
                    //Console.WriteLine(prompt);

                    // Check to send password
                    if (prompt.Contains(":"))
                    {
                            // Send password
                        stream.WriteLine(sensor.password);
                        prompt = stream.Expect(new Regex(@"[$#>]"));
                        //Console.WriteLine(prompt);
                    }

                    var parts = prompt.Trim().Split("\r\n");
                    var values = parts[0].Replace("GiB", "").Replace("MiB", "").Split(" / ");
                    if(parts[0].Contains("MiB")){
                        unit = "MB";
                    }
                    else if(parts[0].Contains("GiB")){
                        unit = "GB";
                    }

                    if(values[0] == "0B"){
                        return new Result(){
                            Latency = stopwatch.ElapsedMilliseconds,
                            Message = "The container is down or not created",
                            Value = -1
                        };
                    }

                    string numberWithoutComma = values[0].Replace(".", ",");

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
