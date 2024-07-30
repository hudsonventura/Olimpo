using System.Net;
using System.Security.Cryptography.X509Certificates;
using Olimpo.Domain;

namespace Olimpo.Sensors;

//TODO: Implements a way to inform the endpoint

public class HTTPS : ISensor
{
    public string GetType()
    {
        return "HTTPS";
    }


    public async Task<List<Channel>> Test(Device service, Sensor sensor){
        return await Check(service, sensor, true);
    }

    protected async Task<List<Channel>> Check(Device service, Sensor sensor, bool SSL_Verification_Check)
    {
        List<Channel> channels = new List<Channel>();
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
            return SSL_Verification_Check;
        }; 


        // Criação do HttpClient
        using (HttpClient client = new HttpClient(handler))
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                string url = $"https://{service.host}:{sensor.port}";
                if(sensor.host != null && sensor.host != string.Empty){
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

            //channels not exists yet
            if(sensor.channels.Count == 0){
                channels.Add(new Channel(){
                    name = "Status code",
                    channel_id = 1,
                    unit = " Status code",
                    current_metric = result
                });

                channels.Add(new Channel(){
                    name = "Days for certifier expire",
                    channel_id = 2,
                    unit = " days",
                    current_metric = new Metric(){
                        message = (expirationDate - DateTime.UtcNow) >= TimeSpan.FromDays(1) ? "Success" : "Error", //Just one day. Change to alerts value
                        value = (expirationDate - DateTime.UtcNow).Days,
                        latency = stopwatch.ElapsedMilliseconds,
                        status = (expirationDate - DateTime.UtcNow) >= TimeSpan.FromDays(1) ? Metric.Status.Success : Metric.Status.Error //Just one day. Change to alerts value
                    }
                });
                return channels;
            }

            //channels already exists
            var channel1 = sensor.channels[0];
            channel1.current_metric = result;
            channels.Add(channel1);

            var channel2 = sensor.channels[1];
            channel2.current_metric = new Metric(){
                message = (expirationDate - DateTime.UtcNow) >= TimeSpan.FromDays(1) ? "Success" : "Error", //Just one day. Change to alerts value
                value = (expirationDate - DateTime.UtcNow).Days,
                latency = stopwatch.ElapsedMilliseconds,
                status = (expirationDate - DateTime.UtcNow) >= TimeSpan.FromDays(1) ? Metric.Status.Success : Metric.Status.Error //Just one day. Change to alerts value
            };
            channels.Add(channel2);
            
            return channels;

        }
    }

}

