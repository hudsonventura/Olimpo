using System.Net;
using System.Security.Cryptography.X509Certificates;
using Olimpo.Domain;

namespace Olimpo.Sensors;


public class HTTPS : SensorGenDefaultChannel, ISensor3
{
    public async Task<List<Channel>> Test(Service service, Sensor sensor)
    {
        List<Channel> channels = new List<Channel>();
        string info = (sensor.SSL_Verification_Check == true) ? "SSL check on" : "SSL check off";
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
            return (bool)sensor.SSL_Verification_Check;
        }; 


        // Criação do HttpClient
        using (HttpClient client = new HttpClient(handler))
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://{service.host}:{sensor.port}");
                
                //get the response 200, 300, 400, 500, etc
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
                
                //get erro to access, like timeout, recuses, etc
                result = new Metric(){
                    message = msg,
                    latency = stopwatch.ElapsedMilliseconds
                };
            }finally{
                stopwatch.Stop();
            }

            channels.Add(new Channel(){
                name = $"{sensor.name} - {info}",
                channel_id = 1,
                current_metric = result
            });

            channels.Add(new Channel(){
                name = $"{sensor.name} - Days for certifier expire",
                channel_id = 2,
                unit = " days",
                current_metric = new Metric(){
                    message = (expirationDate - DateTime.UtcNow) >= TimeSpan.FromDays(1) ? "Ok" : "Error",
                    value = (expirationDate - DateTime.UtcNow).Days,
                    latency = stopwatch.ElapsedMilliseconds
                }
            });
            return channels;

        }
    }



}

