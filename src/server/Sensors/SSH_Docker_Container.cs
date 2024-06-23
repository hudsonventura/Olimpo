using System.Text.RegularExpressions;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSH_Docker_Container : ISensor
{
    public string GetType()
    {
        return "SSH Docker Container";
    }

    public async Task<List<Channel>> Test(Device service, Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();
        string command = $"sudo -S docker stats torrent --no-stream";

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

                    var lines = prompt.Trim().Split("\r\n");
                    var parts = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //value, name, unit
                    List<(decimal, string, string)> elements = DecodeParts(parts);
                    
                    for (int i = 0; i < elements.Count(); i++)
                    {
                        channels.Add(new Channel(){
                                channel_id = i+1,
                                name = $"{sensor.name} - {elements[i].Item2}",
                                unit = elements[i].Item3,
                                current_metric = new Metric(){
                                latency = stopwatch.ElapsedMilliseconds,
                                message = "Success",
                                value = elements[i].Item1,
                                status = Metric.Status.Success
                            }
                        });
                    }


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


        return channels;
    }


    private List<(decimal, string, string)> DecodeParts(string[] parts)
    {
        //value, name, unit
        // 3 - used mememoruy
        // 5 =- memory total
        // 7 - dados baixados na rede
        // 9 - dados enviados na rede
        List<(decimal, string, string)> result = new List<(decimal, string, string)>();

        result.Add((
            decimal.Parse(parts[3].Replace(".", ",").Replace("KiB", "").Replace("MiB", "").Replace("GiB", "").Replace("TiB", "")),
            "Used memory",
            DecodeUnit(parts[3])
        ));
        
        result.Add((
            decimal.Parse(parts[5].Replace(".", ",").Replace("KiB", "").Replace("MiB", "").Replace("GiB", "").Replace("TiB", "")),
            "Total memory",
            DecodeUnit(parts[5])
        ));

        return result;
    }

    private string DecodeUnit(string unit){
        if(unit.Contains("KiB"))
            return "KB";
        if(unit.Contains("MiB"))
            return "MB";
        if(unit.Contains("GiB"))
            return "GB";
        if(unit.Contains("TiB"))
            return "TB";
        return "B";
    }
}
