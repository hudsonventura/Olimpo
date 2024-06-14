using System.Text.RegularExpressions;
using System.Diagnostics;
using Olimpo.Domain;
using Renci.SshNet;
using System.Xml.Linq;
using System.Dynamic;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Olimpo.Sensors;

public class SSH_Advanced_XML_Script : ISensor
{
    public async Task<List<Channel>> Test(Device service, Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();
        string command = $"sudo -S /var/prtg/scriptsxml/disk.sh /mnt/Onedrive";

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
                    var xml = string.Join("", lines);
                    var last_tag = xml.Substring(xml.Length - 7);
                    xml = (last_tag != "</prtg>") ? $"{xml}</prtg>" : xml;
                    
                    XDocument xmlDoc = XDocument.Parse(xml);
                    string json = JsonConvert.SerializeXNode(xmlDoc);
                    var jsonObject = JObject.Parse(json);
                    JArray results = (JArray)jsonObject["prtg"]["result"];
                    
                    int channelId = 0;
                    foreach (var item in results) {
                        channelId++;
                        dynamic result = item.ToString();

                        var channel = GetDynamicPropertyValue(item, "channel");
                        var value = GetDynamicPropertyValue(item, "value");
                        var unit = GetDynamicPropertyValue(item, "CustomUnit");
                        unit = (unit != null) ? unit : GetDynamicPropertyValue(item, "Unit");
                        var LimitMinWarning = GetDynamicPropertyValue(item, "LimitMinWarning");
                        var LimitMinError = GetDynamicPropertyValue(item, "LimitMinError");
                        var LimitMode = GetDynamicPropertyValue(item, "LimitMode");

                        channels.Add(new Channel(){
                                channel_id = channelId,
                                name = channel,
                                unit = unit,
                                current_metric = new Metric(){
                                    latency = stopwatch.ElapsedMilliseconds,
                                    message = "Success",
                                    value = decimal.Parse(value),
                                    status = Metric.Status.Success,
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




    static string GetDynamicPropertyValue(dynamic obj, string propertyName)
    {
        try
        {
            // Tentar obter o valor da propriedade
            return obj[propertyName]?.ToString();
        }
        catch (Exception)
        {
            // Retornar null se a propriedade não existir
            return null;
        }
    }
}
