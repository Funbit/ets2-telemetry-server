using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Funbit.Ets.Telemetry.Server.Data;
using Funbit.Ets.Telemetry.Server.Helpers;
using Newtonsoft.Json;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    [RoutePrefix("api")]
    public class Ets2TelemetryController : ApiController
    {
        public const string TelemetryApiUriPath = "/api/ets2/telemetry";
        const string TestTelemetryJsonFileName = "Ets2TestTelemetry.json";

        static readonly bool UseTestTelemetryData = Convert.ToBoolean(
            ConfigurationManager.AppSettings["UseEts2TestTelemetryData"]);

        public static string GetEts2TelemetryJson()
        {
            // if we have test data defined in the app.config then use it
            if (UseTestTelemetryData)
            {
                using (var file = File.Open(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestTelemetryJsonFileName), 
                        FileMode.Open,
                        FileAccess.Read, 
                        FileShare.ReadWrite))
                using (var reader = new StreamReader(file, Encoding.UTF8))
                    return reader.ReadToEnd();
            }

            // otherwise return real data from the simulator
            return JsonConvert.SerializeObject(Ets2TelemetryDataReader.Instance.Read(), JsonHelper.RestSettings);
        }
        
        [HttpGet]
        [HttpPost]
        [Route("ets2/telemetry", Name = "Get")]
        public HttpResponseMessage Get()
        {
            var telemetryJson = GetEts2TelemetryJson();
            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(telemetryJson, Encoding.UTF8, "application/json");
            response.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };    
            return response;
        }
    }
}