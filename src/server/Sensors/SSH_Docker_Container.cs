using System.Text.RegularExpressions;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSH_DOCKER_CONTAINER : ISensor3
{

    public async Task<List<Channel>> Test(Service service, Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();
        List<(string, Channel)> commands = new List<(string, Channel)>();
        commands.Add(($"sudo -S docker stats torrent --no-stream --format \"{{{{.MemUsage}}}}\" | awk '{{print $1}}'", 
            new Channel(){
                name = $"{sensor.name} - Memory Used",
                channel_id = 1,
        }));
        commands.Add(($"sudo -S docker stats torrent --no-stream --format \"{{{{.MemUsage}}}}\" | awk '{{print $3}}'", 
            new Channel(){
                name = $"{sensor.name} - Memory total",
                channel_id = 1,
        }));


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

                    foreach (var command in commands)
                    {
                        stream.WriteLine(command.Item1);
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
                        decimal value = 0;
                        string unit = "";
                        foreach (var part in parts)
                        {
                            try
                            {
                                string numberWithoutComma = part.Replace(".", ",").Replace("KiB", "").Replace("MiB", "").Replace("GiB", "").Replace("TiB", "");
                                value = decimal.Parse(numberWithoutComma);

                                if(part.Contains("KiB"))
                                    unit = "KB";
                                if(part.Contains("MiB"))
                                    unit = "MB";
                                if(part.Contains("GiB"))
                                    unit = "GB";
                                if(part.Contains("TiB"))
                                    unit = "TB";
                                
                                break;
                            }
                            catch (System.Exception)
                            {
                                
                            }
                        }

                        for (int i = 0; i < sensor.channels.Count; i++)
                        {
                            sensor.channels[i].current_metric = new Metric(){
                                latency = stopwatch.ElapsedMilliseconds,
                                message = "Ok",
                                //value = convertStringToDecimalInBytes(values[i])
                            };
                        }
                        
                    }
                    // Send command and expect password or user prompt
                    
                    //return sensor;
                }
                else
                {
                    Console.WriteLine("Não foi possível conectar ao servidor SSH.");
                }
            }
            catch (Exception ex)
            {
                foreach (var command in commands)
                {
                    command.Item2.current_metric = new Metric(){
                        latency = stopwatch.ElapsedMilliseconds,
                        error_code = 1,
                        message = $"Exception: {ex.Message}",
                    };
                }
            }
            finally
            {
                client.Disconnect();
            }
        }
        
        foreach (var command in commands)
        {
            channels.Add(command.Item2);
        }

        
        return channels;
    }



}
