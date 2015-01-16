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

        static readonly string TelemetryDebugData =
            File.Exists(ConfigurationManager.AppSettings["Ets2TelemetryDebugJsonDataFileName"])
                ? File.ReadAllText(ConfigurationManager.AppSettings["Ets2TelemetryDebugJsonDataFileName"], Encoding.UTF8)
                : null;

        [HttpGet]
        [HttpPost]
        [Route("ets2/telemetry", Name = "GetEts2Telemetry")]
        public HttpResponseMessage GetEts2Telemetry()
        {
            // if we have debug data defined in the config - lets return it then
            if (!string.IsNullOrEmpty(TelemetryDebugData))
            {
                var sampleResponse = Request.CreateResponse(HttpStatusCode.OK);
                sampleResponse.Content = new StringContent(TelemetryDebugData, Encoding.UTF8, "application/json");
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