using Olimpo.Domain;

namespace Olimpo.Sensors;

public class HTTP : ISensor
{
    public async Task<List<Channel>> Test(Service service, Sensor sensor)
    {
        Metric result = null;
        List<Channel> channels = new List<Channel>();

        // Criação do HttpClient
        using (HttpClient client = new HttpClient())
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{service.host}:{sensor.port}");
                result = new Metric(){
                    message = $"Status code: {response.StatusCode.ToString()}",
                    value = (long)response.StatusCode,
                    latency = stopwatch.ElapsedMilliseconds
                };
            }
            catch (Exception error)
            {
                result = new Metric(){
                    message = error.Message,
                    latency = stopwatch.ElapsedMilliseconds,
                    error_code = 1
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
