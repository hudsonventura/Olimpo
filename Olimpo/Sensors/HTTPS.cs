using System.Net;
using System.Security.Cryptography.X509Certificates;
using Olimpo.Domain;

namespace Olimpo.Sensors;


partial class Sensor{
    public bool SSL_Verification_Check { get; set; } = true;
}

public class HTTPS : SensorGenDefaultChannel, ISensor2
{
    public List<Channel> GenChannels(Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();
        string info = (sensor.SSL_Verification_Check == true) ? "SSL check on" : "SSL check off";
        channels.Add(new Channel(){
            name = $"{sensor.name} - {info}",
            channel_id = 1,
        });
        channels.Add(new Channel(){
            name = $"{sensor.name} - Days for certifier expire",
            channel_id = 2,
        });
        return channels;
    }

    


    public async Task<Sensor> Test(Service service, Sensor sensor)
    {
        Metric result = null;

        var handler = new HttpClientHandler();
        DateTime expirationDate = DateTime.UtcNow;
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            // Se quiser, pode verificar algumas propriedades do certificado aqui
            // Se retornar true, o certificado será considerado válido

            // Obtém a data de expiração do certificado
            expirationDate = cert.NotAfter;

            // Retornar true para ignorar a validação do certificado
            return !sensor.SSL_Verification_Check;;
        }; 


        // Criação do HttpClient
        using (HttpClient client = new HttpClient(handler))
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://{service.host}:{sensor.port}");
                
                result = new Metric(){
                    message = response.StatusCode.ToString(),
                    value = (long)response.StatusCode,
                    latency = stopwatch.ElapsedMilliseconds
                };
            }
            catch (Exception error)
            {
                var msg = error.Message;
                if(error.InnerException is not null){
                    msg += error.InnerException.Message;
                }
                

                result = new Metric(){
                    message = msg,
                    latency = stopwatch.ElapsedMilliseconds
                };
            }finally{
                stopwatch.Stop();
            }
            sensor.channels[0].metric = result;
            sensor.channels[1].metric = new Metric(){
                message = "OK",
                value = (expirationDate - DateTime.UtcNow).Days,
                unit = Sensor.MetricUnit.Days.ToString(),
                latency = stopwatch.ElapsedMilliseconds
            };
            return sensor;
        }
    }



}

