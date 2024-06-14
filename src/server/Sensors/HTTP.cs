using Olimpo.Domain;
using Spectre.Console;

namespace Olimpo.Sensors;

//TODO: Implements a way to inform the endpoint
public class HTTP : ISensor
{
    public async Task<List<Channel>> Test(Device service, Sensor sensor)
    {
        Metric result = null;
        List<Channel> channels = new List<Channel>();

        // Criação do HttpClient
        using (HttpClient client = new HttpClient())
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                string url = $"http://{service.host}:{sensor.port}";
                if(sensor.host != string.Empty){
                    url = sensor.host;
                }

                HttpResponseMessage response = response = await client.GetAsync(url);
                

                Metric.Status status = Metric.Status.Success;
                if(!response.IsSuccessStatusCode){
                    status = Metric.Status.Error;
                }
                result = new Metric(){
                    message = $"Status code: {response.StatusCode.ToString()}",
                    value = (long)response.StatusCode,
                    latency = stopwatch.ElapsedMilliseconds,
                    status = status
                };
            }
            catch (Exception error)
            {
                result = new Metric(){
                    message = error.Message,
                    latency = stopwatch.ElapsedMilliseconds,
                    status = Metric.Status.Error
                };
            }finally{
                stopwatch.Stop();
            }
            channels.Add(new Channel(){
                name = $"{sensor.name} - HTTP",
                current_metric = result
            });

            

            
        }
        return channels;
    }
}
