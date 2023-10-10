using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Methods
{
    public class GetDevice
    {
        private readonly IConfiguration _configuration;
        private RegistryManager _registryManger;

        public GetDevice(IConfiguration configuration)
        {
            _configuration = configuration;
            _registryManger = RegistryManager.CreateFromConnectionString(_configuration.GetConnectionString("IotHub"));

        }

        [Function("GetDevice")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "devices/device")] HttpRequestData req)
        {
           
            var response = req.CreateResponse(HttpStatusCode.OK);
           
            string deviceId = req.Query["deviceId"]!;
            if(!string.IsNullOrEmpty(deviceId))
            {
                var device = await _registryManger.GetTwinAsync(deviceId);
                if(device != null)
                {

                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

                    response.WriteString(JsonConvert.SerializeObject(device));

                    return response;
                }
            
            }

            response = req.CreateResponse(HttpStatusCode.BadRequest);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            return response;
           
        }
    }
}
