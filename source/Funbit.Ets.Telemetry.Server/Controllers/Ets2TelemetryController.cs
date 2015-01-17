using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using Funbit.Ets.Telemetry.Server.Data;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    [RoutePrefix("api")]
    public class Ets2TelemetryController : ApiController
    {
        public const string TelemetryApiUriPath = "/api/ets2/telemetry";
        const string TestTelemetryJsonFileName = "Ets2TestTelemetry.json";

        static readonly bool UseTestTelemetryData = !string.IsNullOrWhiteSpace(
            ConfigurationManager.AppSettings["UseEts2TestTelemetryData"]);
        
        [HttpGet]
        [HttpPost]
        [Route("ets2/telemetry", Name = "GetEts2Telemetry")]
        public HttpResponseMessage GetEts2Telemetry()
        {
            // if we have test data defined in the app.config then use it
            if (UseTestTelemetryData)
            {
                string testJsonData = File.ReadAllText(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, TestTelemetryJsonFileName), Encoding.UTF8);
                var sampleResponse = Request.CreateResponse(HttpStatusCode.OK);
                sampleResponse.Content = new StringContent(testJsonData, Encoding.UTF8, "application/json");
                return sampleResponse;
            }

            // otherwise return real data from the simulator
            var response = Request.CreateResponse(HttpStatusCode.OK, 
                Ets2TelemetryDataReader.Instance.Read());
            response.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            return response;
        }
    }
}