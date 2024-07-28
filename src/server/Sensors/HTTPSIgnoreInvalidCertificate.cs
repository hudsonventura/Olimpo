using System.Net;
using System.Security.Cryptography.X509Certificates;
using Olimpo.Domain;

namespace Olimpo.Sensors;

//TODO: Implements a way to inform the endpoint

public class HTTPSIgnoreInvalidCertificate : HTTPS, ISensor
{
    public string GetType()
    {
        return "HTTPS (Ignore Invalid Certificate)";
    }

    public async Task<List<Channel>> Test(Device service, Sensor sensor){
        return await Check(service, sensor, false);
    }

}

