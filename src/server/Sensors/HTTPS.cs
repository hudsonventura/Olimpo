using System.Net;
using System.Security.Cryptography.X509Certificates;
using Olimpo.Domain;

namespace Olimpo.Sensors;

//TODO: Implements a way to inform the endpoint

public class HTTPS : ISensor
{
    public async Task<List<Channel>> Test(Device service, Sensor sensor)
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
                string url = $"https://{service.host}:{sensor.port}";
                if(sensor.host != string.Empty){
                    url = sensor.host;
                }

                HttpResponseMessage response = await client.GetAsync(url);

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
                
                //get the response 200, 300, 400, 500, etc
                result = new Metric(){
                    message = $"Status code: {response.StatusCode.ToString()}",
                    value = (long)response.StatusCode,
                    latency = stopwatch.ElapsedMilliseconds,
                    status = status
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
                    latency = stopwatch.ElapsedMilliseconds,
                    status = Metric.Status.Error
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
                    message = (expirationDate - DateTime.UtcNow) >= TimeSpan.FromDays(1) ? "Success" : "Error",
                    value = (expirationDate - DateTime.UtcNow).Days,
                    latency = stopwatch.ElapsedMilliseconds
                }
            });
            return channels;

        }
    }



}

