using Olimpo.Domain;

namespace Olimpo.Sensors;

public class HTTP : ISensor
{
    public string GetUnit()
    {
        return "";
    }

    public async Task<Result> Test(Service service, Sensor sensor)
    {

        // Criação do HttpClient
        using (HttpClient client = new HttpClient())
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://{service.host}:{sensor.port}");
                return new Result(){
                    Message = response.StatusCode.ToString(),
                    Value = (long)response.StatusCode,
                    Latency = stopwatch.ElapsedMilliseconds
                };
            }
            catch (Exception error)
            {
                return new Result(){
                    Message = error.Message,
                    Latency = stopwatch.ElapsedMilliseconds
                };
            }finally{
                stopwatch.Stop();
            }
        }
    }
}
