using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Funbit.Ets.Telemetry.Server.Data;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    [RoutePrefix("api")]
    public class Ets2TelemetryController : ApiController
    {
        public const string TelemetryApiUriPath = "/api/ets2/telemetry";

        [HttpGet]
        [HttpPost]
        [Route("ets2/telemetry", Name = "GetEts2Telemetry")]
        public HttpResponseMessage GetEts2Telemetry()
        {
            var response = Request.CreateResponse(HttpStatusCode.OK, 
                Ets2TelemetryDataReader.Instance.Read());
            response.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
            return response;
        }
    }
}