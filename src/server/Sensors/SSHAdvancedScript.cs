using System.Diagnostics;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSHAdvancedScript : ISensor
{
    public async Task<List<Channel>> Test(Device service, Sensor sensor)
    {
        
        List<Channel> channels = new List<Channel>();
        List<(int, string, string)> commands = new List<(int, string, string)>();
        commands.Add((1, "SMART", "/var/prtg/scriptsxml/disk.sh sd"));



        using (var client = new SshClient(service.host, sensor.username, sensor.password))
        {
            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                client.Connect();

                foreach (var command in commands){
                    if (client.IsConnected)
                    {

                        var commandResult = client.RunCommand(command.Item3);
                        string numberWithoutComma = commandResult.Result.Replace(".", ",");

                        channels.Add(new Channel(){
                                channel_id = command.Item1,
                                name = $"{sensor.name} - {command.Item2}",
                                unit = "GB",
                                current_metric = new Metric(){
                                latency = stopwatch.ElapsedMilliseconds,
                                message = "Success",
                                value = decimal.Parse(numberWithoutComma)
                            }
                        });

                        
                    }
                    else
                    {
                        channels.Add(new Channel(){
                                channel_id = command.Item1,
                                name = $"{sensor.name} - {command.Item2}",
                                unit = "",
                                current_metric = new Metric(){
                                latency = stopwatch.ElapsedMilliseconds,
                                message = "SSH not connected",
                                value = -1
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                foreach (var command in commands){
                    channels.Add(new Channel(){
                            channel_id = command.Item1,
                            name = $"{sensor.name} - {command.Item2}",
                            unit = "",
                            current_metric = new Metric(){
                            latency = -1,
                            message = $"Fail on SSH connect: {ex.Message}",
                            value = -1
                        }
                    });
                }
            }
            finally
            {
                client.Disconnect();
            }
        }
        throw new NotImplementedException();
        return channels;
    }

}
