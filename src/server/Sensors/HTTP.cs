using Olimpo.Domain;

namespace Olimpo.Sensors;

public class HTTP : SensorGenDefaultChannel, ISensor2
{
    public async Task<Sensor> Test(Service service, Sensor sensor)
    {
        Metric result = null;
        // Criação do HttpClient
        using (HttpClient client = new HttpClient())
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{service.host}:{sensor.port}");
                result = new Metric(){
                    message = response.StatusCode.ToString(),
                    value = (long)response.StatusCode,
                    latency = stopwatch.ElapsedMilliseconds
                };
            }
            catch (Exception error)
            {
                result = new Metric(){
                    message = error.Message,
                    latency = stopwatch.ElapsedMilliseconds
                };
            }finally{
                stopwatch.Stop();
            }
            sensor.channels[0].current_metric = result;
            return sensor;
        }
    }
}
