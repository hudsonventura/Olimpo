using System.Diagnostics;
using Olimpo.Domain;
using Renci.SshNet;

namespace Olimpo.Sensors;

public class SSH_Memory : ISensor
{
    public (string, int, string) GetType()
    {
        return ("Memory (via SSH)", 22,null);
    }

    public async Task<List<Channel>> Test(Device service, Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();
        List<(int, string, string)> commands = new List<(int, string, string)>();
        commands.Add((1, "Memory total", @"free -k | awk '/Mem:/ {printf "" %.2f"", $2/1024/1024}'"));
        commands.Add((2, "Memory used", @"free -k | awk '/Mem:/ {printf "" %.2f"", $3/1024/1024}'"));
        commands.Add((3, "Memory free", @"free -k | awk '/Mem:/ {printf "" %.2f"", $4/1024/1024}'"));



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
                                value = decimal.Parse(numberWithoutComma),
                                status = Metric.Status.Success
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
                                status = Metric.Status.Error
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
                            value = -1,
                            status = Metric.Status.Error
                        }
                    });
                }
            }
            finally
            {
                client.Disconnect();
            }
        }
        return channels;
    }
}
