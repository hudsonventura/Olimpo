using System.Text.RegularExpressions;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSH_DOCKER_CONTAINER : ISensor2
{

    public async Task<Sensor> Test(Service service, Sensor sensor)
    {
        string command = $"sudo -S docker stats torrent --no-stream --format \"{{{{.MemUsage}}}}\"";

        var connectionInfo = new Renci.SshNet.ConnectionInfo(service.host, sensor.username,
                new PasswordAuthenticationMethod(sensor.username, sensor.password));

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        using (var client = new SshClient(connectionInfo))
        {
            try
            {
            
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
                    var values = parts[0].Split(" / ");

                    if(values[0] == "0B"){
                        sensor.channels.ForEach(x => {
                            x.metric = new Metric(){
                                latency = stopwatch.ElapsedMilliseconds,
                                message = "The container is down or not created",
                                value = -1
                            };
                        });
                        return sensor;
                    }

                    for (int i = 0; i < sensor.channels.Count; i++)
                    {
                        sensor.channels[i].metric = new Metric(){
                            latency = stopwatch.ElapsedMilliseconds,
                            message = "Ok",
                            value = convertStringToDecimalInBytes(values[i])
                        };
                    }
                    return sensor;
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
        sensor.channels.ForEach(x => {
            x.metric = new Metric(){
                latency = stopwatch.ElapsedMilliseconds,
                message = "Timeout",
            };
        });

        return sensor;
    }

    public List<Channel> GenChannels(Sensor sensor){
        List<Channel> channels = new List<Channel>();
        channels.Add(new Channel(){
            name = $"{sensor.name} - Memory Used",
            channel_id = 1,
        });
        channels.Add(new Channel(){
            name = $"{sensor.name} - Memory Total",
            channel_id = 2,
        });
        return channels;
    }

    public decimal convertStringToDecimalInBytes(string value){
        int multiply = 1;
        if(value.Contains("MiB")){
            multiply = 1024*1024;
        }
        if(value.Contains("GiB")){
            multiply = 1024*1024*1024;
        }

        string numberWithoutComma = value.Replace(".", ",").Replace("GiB", "").Replace("MiB", "");

        return decimal.Parse(numberWithoutComma)*multiply;
    }
}
